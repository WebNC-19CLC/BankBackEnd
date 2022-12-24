using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.Infrastructure.Domain.Objects.Jobs
{
  public class JobConfiguration
  {
    public string JobName { get; set; } = "Anonymous Job";

    public JobSendMailSetting SendMailToTechnicalUser { get; set; } = JobSendMailSetting.OnError;

    public bool UseSystemUser { get; set; } = true;
  }
}