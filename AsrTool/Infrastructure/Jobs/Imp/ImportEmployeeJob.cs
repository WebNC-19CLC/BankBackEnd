using System.Data.SqlClient;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Domain.Objects.Configurations;
using AsrTool.Infrastructure.Domain.Objects.Jobs;
using AutoMapper;
using Dapper;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TimeZoneConverter;

namespace AsrTool.Infrastructure.Jobs.Imp
{
  public class ImportEmployeeJob : BaseJob
  {
    protected override JobConfiguration JobConfiguration => new()
    {
      JobName = "Import employees from ERP",
      SendMailToTechnicalUser = Domain.Enums.JobSendMailSetting.OnError,
      UseSystemUser = true
    };

    private const int NON_EXIST_ID = 0;

    private string Cron => AppSettings.Jobs.ImportEmployeeCron;

    private static readonly JsonSerializerSettings s_jsonSerializerSettings = new JsonSerializerSettings
    {
      Converters = new List<JsonConverter> { new StringEnumConverter() },
      NullValueHandling = NullValueHandling.Ignore,
      PreserveReferencesHandling = PreserveReferencesHandling.Objects,
      TypeNameHandling = TypeNameHandling.Objects,
      ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
      Formatting = Formatting.Indented
    };

    private Lazy<Dictionary<string, string>> SiteCountryMap =>
      new Lazy<Dictionary<string, string>>(AppSettings
        .LocationMapping
        .SelectMany(x => x.Value.Select(y => new { Site = y.ToLowerInvariant(), Country = x.Key }))
        .GroupBy(x => x.Site, (k, v) => v.First())
        .ToDictionary(x => x.Site, x => x.Country));

    private Lazy<Dictionary<string, TimeZoneData>> CountryTimezoneMap =>
      new(GetSupportedTimeZones().GroupBy(x => x.Country).ToDictionary(x => x.Key.ToLower(), x => x.First()));

    private readonly ILogger<ImportEmployeeJob> _logger;
    private readonly IMapper _mapper;
    private readonly IAsrContext _context;

    public ImportEmployeeJob(
      ILogger<ImportEmployeeJob> logger,
      IUserResolver userResolver,
      IEmailService emailService,
      IOptions<AppSettings> options,
      IMapper mapper,
      IAsrContext context)
      : base(logger, userResolver, emailService, options)
    {
      _logger = logger;
      _mapper = mapper;
      _context = context;
    }

    public override Task Enqueue()
    {
      BackgroundJob.Enqueue<ImportEmployeeJob>(x => x.Execute(JobConfiguration.JobName));
      return Task.CompletedTask;
    }

    public override Task Recurring()
    {
      RecurringJob.AddOrUpdate<ImportEmployeeJob>(x => x.Execute(JobConfiguration.JobName), Cron);
      return Task.CompletedTask;
    }

    protected override async Task ExecuteJob()
    {
      var parseDataResult = GetDataFromErp();
      var erpEmployees = parseDataResult.SuccessRecords.Select(x => x.Current!);
      var (importDataResult, addedEmployees, updatedEmployees) = ImportData(erpEmployees);

      using (var transaction = await _context.BeginTransactionAsync())
      {
        // Push data to DB
        await _context.AddRangeAsync(addedEmployees);
        await _context.UpdateRangeAsync(updatedEmployees);
        await _context.SaveChangesAsync();
        await _context.ClearChangeTracker();

        // Post processing (e.g. set record links)
        var asrEmployees = await _context.Get<Employee>().ToListAsync();
        SetSupervisors(asrEmployees, erpEmployees);
        await _context.UpdateRangeAsync(asrEmployees);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();
      }

      if (parseDataResult.FailedRecords.Any() || importDataResult.FailedRecords.Any())
      {
        SendMailToTechnicalUser(
          $"[EmployeePortal-{AppSettings.Environment}] Job='{JobConfiguration.JobName}' is failed.",
          string.Join("\n----------------------------------------------------------------------------------------------\n",
            parseDataResult.FailedRecords.Select(x => x.Message)
            .Concat(importDataResult.FailedRecords.Select(x => x.Message)).ToArray()));
      }
    }

    private Result<ErpEmployeeObject, ErpEmployee> GetDataFromErp()
    {
      using var connection = new SqlConnection(AppSettings.ErpDbConnectionString);
      string queryString = @$"
        SELECT *
        FROM (
	        SELECT 
		        {GetColumns<ErpEmployeeObject>()},
		        ROW_NUMBER() OVER(PARTITION BY {nameof(ErpEmployeeObject.Emp_Unique_Id)}
              ORDER BY {nameof(ErpEmployeeObject.Emp_Departure_Date)},
                {nameof(ErpEmployeeObject.Acc_Created_On)} DESC) AS RowNum
	        FROM [{AppSettings.ErpEmployeeView}]
	        WHERE {nameof(ErpEmployeeObject.Emp_Unique_Id)} IS NOT NULL
		        AND {nameof(ErpEmployeeObject.Emp_Nb)} > 0
		        AND {nameof(ErpEmployeeObject.Emp_Visa)} <> '{Constants.Model.ADMIN_VISA}'
	        ) AS TEMP
        WHERE TEMP.RowNum = 1";
      var erpObjects = connection.Query<ErpEmployeeObject>(queryString);

      Result<ErpEmployeeObject, ErpEmployee> result = new();
      foreach (var erpObject in erpObjects)
      {
        try
        {
          result.SuccessRecords.Add(
            new ResultRecord<ErpEmployeeObject, ErpEmployee>(
              erpObject,
              _mapper.Map<ErpEmployeeObject, ErpEmployee>(erpObject)));
        }
        catch
        {
          _logger.LogError($"Parse error: {JsonConvert.SerializeObject(erpObject, s_jsonSerializerSettings)}");
          result.FailedRecords.Add(
            new ResultRecord<ErpEmployeeObject, ErpEmployee>(
              erpObject,
              null,
              $"Parse error: {JsonConvert.SerializeObject(erpObject, s_jsonSerializerSettings)}"));
        }
      }

      return result;
    }

    private static string GetColumns<T>()
    {
      var propInfos = typeof(T).GetProperties().Where(x => x.CanRead && x.CanWrite);
      List<string> columns = new();

      foreach (var prop in propInfos)
      {
        string propName = prop.Name;
        string columnName = $"{propName} AS {propName}";
        if (prop.CustomAttributes.Any(x => x.GetType().IsAssignableFrom(typeof(DateTimeData))))
        {
          columnName = $"FORMAT({propName}, 'yyyy-MM-dd HH:mm:ss') AS {propName}";
        }
        columns.Add(columnName);
      }

      return string.Join(", ", columns);
    }

    private (
      Result<ErpEmployee, Employee> importDataResult,
      ICollection<Employee> addedEmployees,
      ICollection<Employee> updatedEmployees) ImportData(IEnumerable<ErpEmployee> erpEmployees)
    {
      Result<ErpEmployee, Employee> importResult = new();
      List<Employee> addedEmployees = new();
      List<Employee> updatedEmployees = new();

      var asrEmployeeDictionary = _context.Get<Employee>().ToDictionary(x => x.UniqueId);

      foreach (ErpEmployee erpEmp in erpEmployees)
      {
        try
        {
          if (asrEmployeeDictionary.ContainsKey(erpEmp.Emp_Unique_Id))
          {
            var existingAsrEmployee = asrEmployeeDictionary[erpEmp.Emp_Unique_Id];
            var asrEmp = MergeEmployee(existingAsrEmployee, erpEmp);
            asrEmp.Id = existingAsrEmployee.Id;
            asrEmp.RoleId = existingAsrEmployee.RoleId;
            updatedEmployees.Add(asrEmp);
          }
          else if (erpEmp.Emp_Is_Active)
          {
            var asrEmp = MergeEmployee(new Employee(), erpEmp);
            asrEmp.Id = NON_EXIST_ID;
            addedEmployees.Add(asrEmp);
          }
        }
        catch
        {
          _logger.LogError($"Import error: ErpEmployee = {JsonConvert.SerializeObject(erpEmp)}");
          importResult.FailedRecords.Add(
            new ResultRecord<ErpEmployee, Employee>(
              erpEmp,
              null,
              $"Import error: ErpEmployee = {JsonConvert.SerializeObject(erpEmp)}"));
        }
      }

      return (importResult, addedEmployees, updatedEmployees);
    }

    private Employee MergeEmployee(Employee existingAsrEmployee, ErpEmployee erpEmp)
    {
      var mergedEmployee = _mapper.Map<ErpEmployee, Employee>(erpEmp, existingAsrEmployee);
      mergedEmployee.TimeZoneId = GetTimeZoneId(erpEmp.Emp_Site);

      if (!string.IsNullOrEmpty(erpEmp.Emp_Legal_Unit))
      {
        var letterIdentityCount = 2;
        var companyCode = erpEmp.Emp_Legal_Unit[..letterIdentityCount];
        if (AppSettings.LegalUnitMapping.ContainsKey(companyCode))
        {
          mergedEmployee.LegalUnit = AppSettings.LegalUnitMapping[companyCode];
        }
      }

      return mergedEmployee;
    }

    private IEnumerable<TimeZoneData> GetSupportedTimeZones()
    {
      var timeZoneData = new HashSet<TimeZoneData>();
      foreach (var countryTz in AppSettings.TimeZoneMapping)
      {
        var country = countryTz.Key;

        foreach (var tz in countryTz.Value)
        {
          var tzInfo = TZConvert.GetTimeZoneInfo(tz);
          timeZoneData.Add(new TimeZoneData
          {
            TimeZoneId = tz,
            Country = country,
            Description = tzInfo.DisplayName
          });
        }
      }

      return timeZoneData.OrderBy(tz => TZConvert.GetTimeZoneInfo(tz.TimeZoneId).BaseUtcOffset);
    }

    private string GetTimeZoneId(string? site)
    {
      var defaultTimeZoneId = Constants.TimeZoneId.DEFAULT;

      if (string.IsNullOrEmpty(site))
      {
        return defaultTimeZoneId;
      }

      string normalizedSite = site.ToLower();
      if (!SiteCountryMap.Value.ContainsKey(normalizedSite))
      {
        return defaultTimeZoneId;
      }

      string normalizedCountry = SiteCountryMap.Value[normalizedSite].ToLower();
      if (!CountryTimezoneMap.Value.ContainsKey(normalizedCountry))
      {
        return defaultTimeZoneId;
      }

      return CountryTimezoneMap.Value[normalizedCountry].TimeZoneId;
    }

    private void SetSupervisors(ICollection<Employee> asrEmployees, IEnumerable<ErpEmployee> erpEmployees)
    {
      var employees = asrEmployees.ToDictionary(x => x.UniqueId);
      var supervisors = asrEmployees.Where(x => x.Active).ToDictionary(x => x.Visa);

      foreach (var erpEmployee in erpEmployees.Where(x => !string.IsNullOrWhiteSpace(x.Emp_Manager_Visa)))
      {
        if (!employees.TryGetValue(erpEmployee.Emp_Unique_Id, out var asrEmployee))
        {
          continue;
        }

        if (!supervisors.TryGetValue(erpEmployee.Emp_Manager_Visa, out var supervisor))
        {
          continue;
        }

        asrEmployee.SupervisorId = supervisor.Id;
      }
    }

    private record ResultRecord<TOld, TNew>(TOld? Original, TNew? Current, string? Message = null)
    {
    }

    private record Result<TOld, TNew>
    {
      public ICollection<ResultRecord<TOld, TNew>> SuccessRecords { get; set; } =
        new List<ResultRecord<TOld, TNew>>();

      public ICollection<ResultRecord<TOld, TNew>> FailedRecords { get; set; } =
        new List<ResultRecord<TOld, TNew>>();
    }
  }
}
