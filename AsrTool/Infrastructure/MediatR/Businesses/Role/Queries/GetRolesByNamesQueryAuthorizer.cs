using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Queries
{
  public class GetRolesByNamesQueryAuthorizer : IAuthorizer<GetRolesByNamesQuery>
  {
    private readonly IUserResolver _userResolver;

    public GetRolesByNamesQueryAuthorizer(IUserResolver userResolver)
    {
      _userResolver = userResolver;
    }

    public Task<AuthorizationResult> AuthorizeAsync(GetRolesByNamesQuery instance, CancellationToken cancellation = default)
    {
      if (_userResolver.CurrentUser.IsContainsAnyRights(Right.ReadRole, Right.ReadRoleAll))
      {
        return Task.FromResult(AuthorizationResult.Success());
      }
      return Task.FromResult(AuthorizationResult.Failed());
    }
  }
}
