namespace AsrTool.Infrastructure.Auth
{
  public interface ILdapConnector
  {
    bool ValidateSignature(string username, string password);
  }
}