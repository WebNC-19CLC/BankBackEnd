using System.Security.Claims;
using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.Extensions;

namespace AsrTool.Infrastructure.Auth
{
  public class User : IUser
  {
    public User(ClaimsPrincipal? principal, ICacheService cache)
    {
      if (principal?.Identity == null || cache == null || string.IsNullOrWhiteSpace(principal.Identity.Name))
      {
        return;
      }

      Principal = principal;
      Username = principal.Identity.Name;
      Rights = principal.Claims.Where(x => x.Type == Constants.Auth.CLAIM_APP_NAME).Select(x => Enum.Parse<Right>(x.Value)).ToArray();
      AuthType = Principal.Claims.Single(x => x.Type == Constants.Auth.AUTHENTICATION_CLAIMS_TYPE).Value;

      var employeeItem = cache.GetEmployeeCachingItem(Username).RunAwait();
      Id = employeeItem.Id;
      FirstName = employeeItem.FirstName;
      LastName = employeeItem.LastName;
      Visa = employeeItem.Visa;
      LegalUnit = employeeItem.LegalUnit;
      Site = employeeItem.Site;
      OrganizationUnit = employeeItem.OrganizationUnit;
      Department = employeeItem.Department;
      Level = employeeItem.Level;
      RoleName = employeeItem.RoleName ?? string.Empty;
      TimeZoneId = employeeItem.TimeZoneId ?? Constants.TimeZoneId.DEFAULT;
    }

    public int Id { get; }

    public string Username { get; } = default!;

    public string? FirstName { get; }

    public string? LastName { get; }

    public string Visa { get; } = default!;

    public string FullName => string.Join(Constants.Model.NAME_SEPARATOR, FirstName, LastName);

    public string? LegalUnit { get; }

    public string? Site { get; }

    public string? OrganizationUnit { get; }

    public string? Department { get; }

    public int? Level { get; }

    public string RoleName { get; } = default!;

    public string TimeZoneId { get; } = default!;

    public ClaimsPrincipal Principal { get; } = default!;

    public IReadOnlyCollection<Right> Rights { get; } = Array.Empty<Right>();

    public string AuthType { get; } = default!;

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
