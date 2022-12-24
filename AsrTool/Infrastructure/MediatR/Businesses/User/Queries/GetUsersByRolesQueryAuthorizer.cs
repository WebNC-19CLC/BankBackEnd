using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Queries
{
  public class GetUsersByRolesQueryAuthorizer : IAuthorizer<GetUsersByRolesQuery>
  {
    private readonly IUserResolver _userResolver;

    public GetUsersByRolesQueryAuthorizer(IUserResolver userResolver)
    {
      _userResolver = userResolver;
    }

    public Task<AuthorizationResult> AuthorizeAsync(GetUsersByRolesQuery instance, CancellationToken cancellation = default)
    {
      if (_userResolver.CurrentUser.IsContainsAnyRights(new Right[] { Right.ReadRoleAll }))
      {
        return Task.FromResult(AuthorizationResult.Success());
      }
      return Task.FromResult(AuthorizationResult.Failed());
    }
  }
}
