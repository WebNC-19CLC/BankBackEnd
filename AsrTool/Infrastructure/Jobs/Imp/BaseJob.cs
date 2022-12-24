using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.Domain.Objects.Configurations;
using AsrTool.Infrastructure.Domain.Objects.Jobs;
using Hangfire;
using Microsoft.Extensions.Options;

namespace AsrTool.Infrastructure.Jobs.Imp
{
  public abstract class BaseJob : IRecurringJob
  {
    private readonly ILogger _logger;
    protected readonly AppSettings AppSettings;
    protected readonly IUserResolver UserResolver;
    protected readonly IEmailService EmailService;

    protected BaseJob(
      ILogger logger, IUserResolver userResolver, IEmailService emailService, IOptions<AppSettings> options)
    {
      _logger = logger;
      UserResolver = userResolver;
      EmailService = emailService;
      AppSettings = options.Value;
    }

    protected virtual JobConfiguration JobConfiguration { get; } = new JobConfiguration();

    public abstract Task Enqueue();

    public abstract Task Recurring();

    [JobDisplayName("{0}")]
    public async Task Execute(string jobName = null)
    {
      ISystemUserScope scope = null;
      if (JobConfiguration.UseSystemUser)
      {
        _logger.LogInformation($"Run job='{JobConfiguration.JobName}' with system admin");
        scope = UserResolver.UseSystemUser();
      }

      try
      {
        _logger.LogInformation($"Running job='{JobConfiguration.JobName}' with system admin");
        await ExecuteJob();
        if (JobConfiguration.SendMailToTechnicalUser.HasFlag(JobSendMailSetting.OnSuccess))
        {
          SendMailToTechnicalUser($"[AsrTool-{AppSettings.Environment}] Job='{JobConfiguration.JobName}' is done.",
            $"[AsrTool] Job='{JobConfiguration.JobName}' is done.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Job='{GetType().Name}' failed");
        if (!JobConfiguration.SendMailToTechnicalUser.HasFlag(JobSendMailSetting.OnError))
        {
          throw;
        }

        SendMailToTechnicalUser($"[AsrTool-{AppSettings.Environment}] Job='{JobConfiguration.JobName}' is failed.",
          $"[AsrTool] Job='{JobConfiguration.JobName}' is failed. Please check following log for more detail:{Environment.NewLine}{ex}");
        throw;
      }
      finally
      {
        scope?.Dispose();
        _logger.LogInformation($"Finished job={JobConfiguration.JobName}");
      }
    }

    protected abstract Task ExecuteJob();

    protected void SendMailToTechnicalUser(string subject, string body)
    {
      var definedTechnicalUsers = AppSettings.TechnicalUsersCollection;
      try
      {
        EmailService.Send(subject, body, definedTechnicalUsers, isHtml: false);
      }
      catch (Exception emailException)
      {
        _logger.LogError(emailException,
          $"Job='{JobConfiguration.JobName}', can not send mail to technical users ${string.Join(", ", definedTechnicalUsers)}");
      }
    }
  }
}