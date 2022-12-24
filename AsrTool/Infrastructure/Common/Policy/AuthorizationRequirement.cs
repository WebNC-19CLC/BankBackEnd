using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.Common.Policy
{
  public class AuthorizationRequirement : IAuthorizationRequirement
  {
    public static readonly AuthorizationRequirement Read = new AuthorizationRequirement(nameof(Read));
    public static readonly AuthorizationRequirement Create = new AuthorizationRequirement(nameof(Create));
    public static readonly AuthorizationRequirement Update = new AuthorizationRequirement(nameof(Update));
    public static readonly AuthorizationRequirement Delete = new AuthorizationRequirement(nameof(Delete));

    public static readonly AuthorizationRequirement ResetRights = new AuthorizationRequirement(nameof(ResetRights));

    private AuthorizationRequirement(string name)
    {
      Name = name;
    }

    public string Name { get; }

    public override bool Equals(object obj)
    {
      return obj is AuthorizationRequirement r && r.Name == Name;
    }

    public override int GetHashCode()
    {
      return Name.GetHashCode();
    }
  }
}