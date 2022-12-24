using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Commands
{
  public class LogoutCommandAuthorizer : IAuthorizer<LogoutCommand>
  {

    public LogoutCommandAuthorizer()
    {
    }

    public Task<AuthorizationResult> AuthorizeAsync(LogoutCommand instance, CancellationToken cancellation = default)
    {
      if (instance.HttpContext.User.Identity?.IsAuthenticated == true)
      {
        return Task.FromResult(AuthorizationResult.Success());
      }
      return Task.FromResult(AuthorizationResult.Failed());
    }
  }
}