using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Queries
{
  public class SearchUsersByTermQueryAuthorizer : IAuthorizer<SearchUsersByTermQuery>
  {
    public Task<AuthorizationResult> AuthorizeAsync(SearchUsersByTermQuery instance, CancellationToken cancellation = default)
    {
      return Task.FromResult(AuthorizationResult.Success());
    }
  }
}
