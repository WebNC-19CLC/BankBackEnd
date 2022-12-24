using System.Security.Claims;
using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.Infrastructure.Auth
{
  public interface IUser
  {
    int Id { get; }

    string Username { get; }

    string? FirstName { get; }

    string? LastName { get; }

    string Visa { get; }

    string FullName { get; }

    string? LegalUnit { get; }

    string? Site { get; }

    string? OrganizationUnit { get; }

    string? Department { get; }

    int? Level { get; }

    string RoleName { get; }

    string TimeZoneId { get; }

    ClaimsPrincipal Principal { get; }

    IReadOnlyCollection<Right> Rights { get; }

    string AuthType { get; }

    bool IsContainsAnyRights(params Right[] rights);

    bool IsContainsAllRights(params Right[] rights);
  }
}
