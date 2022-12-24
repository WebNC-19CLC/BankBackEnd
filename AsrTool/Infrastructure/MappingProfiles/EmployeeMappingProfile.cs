using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.Domain.Objects.Jobs;

namespace AsrTool.Infrastructure.MappingProfiles
{
  public class EmployeeMappingProfile : BaseMappingProfile<Employee>
  {
    public EmployeeMappingProfile()
    {
      CreateMap<ErpEmployeeObject, ErpEmployee>()
        .ForMember(des => des.Emp_Unique_Id, opt => opt.MapFrom(src => Parse<int>(src.Emp_Unique_Id)))
        .ForMember(des => des.Emp_Nb, opt => opt.MapFrom(src => Parse<int?>(src.Emp_Nb)))
        .ForMember(des => des.Emp_FirstName, opt => opt.MapFrom(src => Parse<string?>(src.Emp_FirstName)))
        .ForMember(des => des.Emp_LastName, opt => opt.MapFrom(src => Parse<string?>(src.Emp_LastName)))
        .ForMember(des => des.Emp_Birthday_Date, opt => opt.MapFrom(src => Parse<DateTime?>(src.Emp_Birthday_Date)))
        .ForMember(des => des.Emp_Arrival_Date, opt => opt.MapFrom(src => Parse<DateTime?>(src.Emp_Arrival_Date)))
        .ForMember(des => des.Emp_Visa, opt => opt.MapFrom(src => Parse<string?>(src.Emp_Visa)))
        .ForMember(des => des.Acc_Email, opt => opt.MapFrom(src => Parse<string?>(src.Acc_Email)))
        .ForMember(des => des.Emp_Gender, opt => opt.MapFrom(src => Parse<string>(src.Emp_Gender)))
        .ForMember(des => des.Emp_Level, opt => opt.MapFrom(src => Parse<int?>(src.Emp_Level)))
        .ForMember(des => des.Emp_ADDomain, opt => opt.MapFrom(src => Parse<string?>(src.Emp_ADDomain)))
        .ForMember(des => des.Emp_Org_Unit, opt => opt.MapFrom(src => Parse<string?>(src.Emp_Org_Unit)))
        .ForMember(des => des.Emp_Head_Unit_Visa, opt => opt.MapFrom(src => Parse<string?>(src.Emp_Head_Unit_Visa)))
        .ForMember(des => des.Emp_Is_Active, opt => opt.MapFrom(src => Parse<bool>(src.Emp_Is_Active)))
        .ForMember(des => des.Emp_Head_Operation_Visa, opt => opt.MapFrom(src => Parse<string?>(src.Emp_Head_Operation_Visa)))
        .ForMember(des => des.Emp_Site, opt => opt.MapFrom(src => Parse<string?>(src.Emp_Site)))
        .ForMember(des => des.Emp_Department, opt => opt.MapFrom(src => Parse<string?>(src.Emp_Department)))
        .ForMember(des => des.Emp_Role, opt => opt.MapFrom(src => Parse<string?>(src.Emp_Role)))
        .ForMember(des => des.Emp_Manager_Id, opt => opt.MapFrom(src => Parse<string?>(src.Emp_Manager_Id)))
        .ForMember(des => des.Tenure_Month, opt => opt.MapFrom(src => Parse<int?>(src.Tenure_Month)))
        .ForMember(des => des.Emp_Id, opt => opt.MapFrom(src => Parse<int>(src.Emp_Id)))
        .ForMember(des => des.Emp_Manager_Nb, opt => opt.MapFrom(src => Parse<int?>(src.Emp_Manager_Nb)))
        .ForMember(des => des.Emp_Manager_Visa, opt => opt.MapFrom(src => Parse<string?>(src.Emp_Manager_Visa)))
        .ForMember(des => des.Emp_Legal_Unit, opt => opt.MapFrom(src => Parse<string?>(src.Emp_Legal_Unit)))
        .ForMember(des => des.Emp_Departure_Date, opt => opt.MapFrom(src => Parse<DateTime?>(src.Emp_Departure_Date)))
        .ForMember(des => des.Acc_Created_On, opt => opt.MapFrom(src => Parse<DateTime?>(src.Acc_Created_On)));

      CreateMap<ErpEmployee, Employee>()
        .ForMember(des => des.Username, opt => opt.MapFrom(src => Constants.Auth.DOMAIN + "\\" + src.Emp_Visa))
        .ForMember(des => des.UniqueId, opt => opt.MapFrom(src => src.Emp_Unique_Id))
        .ForMember(des => des.AdDomain, opt => opt.MapFrom(src => src.Emp_ADDomain))
        .ForMember(des => des.FirstName, opt => opt.MapFrom(src => src.Emp_FirstName))
        .ForMember(des => des.LastName, opt => opt.MapFrom(src => src.Emp_LastName))
        .ForMember(des => des.Gender, opt => opt.MapFrom(src => src.Emp_Gender == "M" ? Gender.Male : Gender.Female))
        .ForMember(des => des.BirthDate, opt => opt.MapFrom(src => src.Emp_Birthday_Date))
        .ForMember(des => des.Visa, opt => opt.MapFrom(src => src.Emp_Visa))
        .ForMember(des => des.Email, opt => opt.MapFrom(src => src.Acc_Email))
        .ForMember(des => des.EntryDate, opt => opt.MapFrom(src => src.Emp_Arrival_Date))
        //.ForMember(des => des.DiplomaDate, opt => opt.MapFrom(src => src.)) // TODO: CHECK MAPPING
        .ForMember(des => des.Site, opt => opt.MapFrom(src => src.Emp_Site))
        .ForMember(des => des.OrganizationUnit, opt => opt.MapFrom(src => src.Emp_Org_Unit))
        .ForMember(des => des.Department, opt => opt.MapFrom(src => src.Emp_Department))
        .ForMember(des => des.HeadUnitVisa, opt => opt.MapFrom(src => src.Emp_Head_Unit_Visa))
        .ForMember(des => des.HeadOperationVisa, opt => opt.MapFrom(src => src.Emp_Head_Operation_Visa))
        .ForMember(des => des.TechnicalRole, opt => opt.MapFrom(src => src.Emp_Role))
        .ForMember(des => des.Level, opt => opt.MapFrom(src => src.Emp_Level))
        //.ForMember(des => des.JobCode, opt => opt.MapFrom(src => src.)) // TODO: CHECK MAPPING
        .ForMember(des => des.ElcaTenureMonth, opt => opt.MapFrom(src => src.Tenure_Month))
        .ForMember(des => des.Active, opt => opt.MapFrom(src => src.Emp_Is_Active));

      CreateMap<Employee, UserByRoleDto>()
        .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(des => des.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
        .ForMember(des => des.Visa, opt => opt.MapFrom(src => src.Visa))
        .ForMember(des => des.TechnicalRole, opt => opt.MapFrom(src => src.TechnicalRole))
        .ForMember(des => des.Level, opt => opt.MapFrom(src => src.Level))
        .ForMember(des => des.LegalUnit, opt => opt.MapFrom(src => src.LegalUnit))
        .ForMember(des => des.WorkLocation, opt => opt.MapFrom(src => src.Site))
        .ForMember(des => des.RoleId, opt => opt.MapFrom(src => src.RoleId));
    }

    private static T? Parse<T>(string? str)
    {
      if (typeof(T) == typeof(string))
      {
        return !string.IsNullOrEmpty(str) ? (T?)(object?)str : default!;
      }

      if (typeof(T) == typeof(int))
      {
        return !string.IsNullOrWhiteSpace(str) ? (T)(object)int.Parse(str) : default!;
      }

      if (typeof(T) == typeof(int?))
      {
        return !string.IsNullOrWhiteSpace(str) ? (T?)(object?)int.Parse(str) : (T?)(object?)null;
      }

      if (typeof(T) == typeof(DateTime))
      {
        return !string.IsNullOrWhiteSpace(str) ? (T)(object)DateTime.Parse(str) : default!;
      }

      if (typeof(T) == typeof(DateTime?))
      {
        return !string.IsNullOrWhiteSpace(str) ? (T)(object)DateTime.Parse(str) : default!;
      }

      if (typeof(T) == typeof(bool))
      {
        if (str == "1")
        {
          return (T)(object)true;
        }
        if (str == "0")
        {
          return (T)(object)false;
        }
        return !string.IsNullOrWhiteSpace(str) ? (T)(object)bool.Parse(str) : default!;
      }

      throw new NotSupportedException($"Type={typeof(T).Name} is not supported");
    }
  }
}