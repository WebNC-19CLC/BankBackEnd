using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Commands
{
  public class AssignUsersToRoleCommandAuthorizer : IAuthorizer<AssignUsersToRoleCommand>
  {
    private readonly IUserResolver _userResolver;

    public AssignUsersToRoleCommandAuthorizer(IUserResolver userResolver)
    {
      _userResolver = userResolver;
    }

    public Task<AuthorizationResult> AuthorizeAsync(AssignUsersToRoleCommand instance, CancellationToken cancellation = default)
    {
      if (_userResolver.CurrentUser.IsContainsAnyRights(new Right[] { Right.WriteRole, Right.WriteRoleAll }))
      {
        return Task.FromResult(AuthorizationResult.Success());
      }
      return Task.FromResult(AuthorizationResult.Failed());
    }
  }
}
