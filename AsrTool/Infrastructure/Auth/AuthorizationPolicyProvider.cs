using AsrTool.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.Auth
{
  public class AuthorizationPolicyProvider : IAuthorizationPolicyProvider
  {
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
      return Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
      return Task.FromResult<AuthorizationPolicy?>(null);
    }

    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
      if (!policyName.StartsWith(Constants.Auth.CLAIM_NAME, StringComparison.Ordinal))
      {
        throw new NotSupportedException("Unsupported policy: " + policyName);
      }

      var rights = policyName.Substring(Constants.Auth.CLAIM_NAME.Length).DeserializePermission();
      return Task.FromResult(
        new AuthorizationPolicyBuilder()
          .RequireClaim(Constants.Auth.CLAIM_APP_NAME, rights.Select(x => $"{x}").ToArray())
          .Build());
    }
  }
}