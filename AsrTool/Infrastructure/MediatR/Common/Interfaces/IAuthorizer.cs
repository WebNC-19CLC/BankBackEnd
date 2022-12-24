using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.MediatR.Common.Interfaces
{
  public interface IAuthorizer<T>
  {
    Task<AuthorizationResult> AuthorizeAsync(T instance, CancellationToken cancellation = default);
  }
}
