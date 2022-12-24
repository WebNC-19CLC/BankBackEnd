namespace AsrTool.Infrastructure.Domain.Enums
{
  [Flags]
  public enum JobSendMailSetting
  {
    Ignore,
    OnError,
    OnSuccess,
    Always,
  }
}