using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AsrTool.UnitTest._Common.Fake
{
  public class FakeUnitTestUser : IUser
  {
    public int Id { get; set; }
    public string Username { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Visa { get; set; } = default!;
    public string FullName => string.Join(Constants.Model.NAME_SEPARATOR, FirstName, LastName);
    public string? LegalUnit { get; set; }
    public string? Site { get; set; }
    public string? OrganizationUnit { get; set; }
    public string? Department { get; set; }
    public int? Level { get; set; }
    public string RoleName { get; set; } = default!;
    public string TimeZoneId { get; set; } = default!;
    public ClaimsPrincipal Principal { get; set; }
    public IReadOnlyCollection<Right> Rights { get; set; }
    public string AuthType { get; set; }

    public bool IsContainsAnyRights(params Right[] rights)
    {
      if (rights == null || !rights.Any())
      {
        return true;
      }

      return rights.Any(Rights.Contains);
    }

    public bool IsContainsAllRights(params Right[] rights)
    {
      if (rights == null || !rights.Any())
      {
        return true;
      }

      return !rights.Except(Rights).Any();
    }
  }
}