using AsrTool.Infrastructure.Domain.Objects.Configurations;
using AsrTool.Infrastructure.Exceptions;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;

namespace AsrTool.Infrastructure.Auth
{
  public class LdapConnector : ILdapConnector, IDisposable
  {
    private readonly ILogger<LdapConnector> _logger;
    private readonly LdapConnection _ldapConnection;
    private readonly AppSettings _settings;

    public LdapConnector(ILogger<LdapConnector> logger, IOptions<AppSettings> settings)
    {
      _logger = logger;
      _ldapConnection = new LdapConnection();
      _settings = settings.Value;
    }

    public bool ValidateSignature(string username, string password)
    {
#if DEBUG || STAGING
      return true;
#endif

      try
      {
        if (!_ldapConnection.Connected)
        {
          _logger.LogInformation($"Connect to '{_settings.LdapSettings.Domain}:{LdapConnection.DefaultPort}'");
          _ldapConnection.Connect(_settings.LdapSettings.Domain, LdapConnection.DefaultPort);
        }
        _ldapConnection.Bind(username, password);
        return true;
      }
      catch (LdapException ex)
      {
        _logger.LogError(ex, $"Connect to user='{username}' failed");
        throw new BusinessException("Username or password is not correct, please try again", ex);
      }
    }

    public void Dispose()
    {
      _ldapConnection?.Dispose();
      _logger.LogDebug("Disposed Ldap Connection");
    }
  }
}