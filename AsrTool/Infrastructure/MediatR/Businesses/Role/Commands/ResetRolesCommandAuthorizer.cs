using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Commands
{
  public class ResetRolesCommandAuthorizer : IAuthorizer<ResetRolesCommand>
  {
    private readonly IUserResolver _userResolver;

    public ResetRolesCommandAuthorizer(IUserResolver userResolver)
    {
      _userResolver = userResolver;
    }

    public Task<AuthorizationResult> AuthorizeAsync(ResetRolesCommand instance, CancellationToken cancellation = default)
    {
      if (_userResolver.CurrentUser.IsContainsAnyRights(new Right[] { Right.ResetRights }))
      {
        return Task.FromResult(AuthorizationResult.Success());
      }
      return Task.FromResult(AuthorizationResult.Failed());
    }
  }
}
