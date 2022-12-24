using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Queries
{
  public class GetCurrentUserQueryAuthorizer : IAuthorizer<GetCurrentUserQuery>
  {
    private readonly IUserResolver _userResolver;

    public GetCurrentUserQueryAuthorizer(IUserResolver userResolver)
    {
      _userResolver = userResolver;
    }

    public Task<AuthorizationResult> AuthorizeAsync(GetCurrentUserQuery instance, CancellationToken cancellation = default)
    {
      if (_userResolver.CurrentUser != null)
      {
        return Task.FromResult(AuthorizationResult.Success());
      }

      return Task.FromResult(AuthorizationResult.Failed());
    }
  }
}