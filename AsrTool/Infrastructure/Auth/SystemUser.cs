using System.Security.Claims;
using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.Infrastructure.Auth
{
  public class SystemUser : IUser
  {
    public int Id => -1;

    public string Username => "SYSTEM";

    public string? FirstName => "System";

    public string? LastName => "Admin";

    public string Visa => "SYSTEM";

    public string FullName => string.Join(Constants.Model.NAME_SEPARATOR, FirstName, LastName);

    public string? LegalUnit => string.Empty;

    public string? OrganizationUnit => string.Empty;

    public string? Department => string.Empty;

    public string? Site => string.Empty;

    public int? Level => 0;

    public string RoleName => Constants.Roles.Admin;

    public string TimeZoneId => Constants.TimeZoneId.DEFAULT;

    public ClaimsPrincipal Principal => new ClaimsPrincipal();

    public IReadOnlyCollection<Right> Rights => Enum.GetValues<Right>().ToArray();

    public string AuthType => Constants.Auth.WINDOW_AUTHENTICATION;

    public bool IsContainsAnyRights(params Right[] rights)
    {
      return true;
    }

    public bool IsContainsAllRights(params Right[] rights)
    {
      return true;
    }
  }
}
