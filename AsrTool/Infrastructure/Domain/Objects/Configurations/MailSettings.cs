namespace AsrTool.Infrastructure.Domain.Objects.Configurations
{
  public class MailSettings
  {
    public string From { get; set; } = default!;

    public string? Server { get; set; }

    public int Port { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }
  }
}