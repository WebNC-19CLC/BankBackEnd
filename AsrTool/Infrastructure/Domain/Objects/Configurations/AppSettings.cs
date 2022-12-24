using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.Infrastructure.Domain.Objects.Configurations
{
  public class AppSettings
  {
    public string AsrToolDbConnectionString { get; set; } = default!;

    public string HangfireDbConnectionString { get; set; } = default!;

    public string ErpDbConnectionString { get; set; } = default!;

    public string ErpEmployeeView { get; set; } = default!;

    public ApplicationEnvironment Environment { get; set; }

    public string? KeyPath { get; set; }

    public LdapSettings LdapSettings { get; set; }

    public MailSettings MailSettings { get; set; }

    public JobSettings Jobs { get; set; }

    public IDictionary<string, string> LegalUnitMapping { get; set; } = new Dictionary<string, string>();

    public IDictionary<string, HashSet<string>> LocationMapping { get; set; } = new Dictionary<string, HashSet<string>>();

    public IDictionary<string, HashSet<string>> TimeZoneMapping { get; set; } = new Dictionary<string, HashSet<string>>();

    /// <summary>
    /// Please don't change it to array, the configuration of array in config file is append.
    /// It's not replaced.
    /// https://github.com/dotnet/runtime/issues/36569#
    /// </summary>
    public string TechnicalUsers { get; set; } = string.Empty;

    public IReadOnlyCollection<string> TechnicalUsersCollection => TechnicalUsers.Split(";", StringSplitOptions.RemoveEmptyEntries);

    public bool ActivateEncryption =>
      !string.IsNullOrWhiteSpace(KeyPath) && (Environment == ApplicationEnvironment.Prod || Environment == ApplicationEnvironment.Staging);
  }
}
