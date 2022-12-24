using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.MediatR.Businesses.ReferenceData.Queries
{
  public class GetReferenceDataQueryAuthorizer : IAuthorizer<GetReferenceDataQuery>
  {
    private readonly IUserResolver _userResolver;

    public GetReferenceDataQueryAuthorizer(IUserResolver userResolver)
    {
      _userResolver = userResolver;
    }

    public Task<AuthorizationResult> AuthorizeAsync(GetReferenceDataQuery instance, CancellationToken cancellation = default)
    {
      if (_userResolver.CurrentUser != null)
      {
        return Task.FromResult(AuthorizationResult.Success());
      }

      return Task.FromResult(AuthorizationResult.Failed());
    }
  }
}
