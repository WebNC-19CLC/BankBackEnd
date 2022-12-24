using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Commands
{
  public class LoginCommandAuthorizer : IAuthorizer<LoginCommand>
  {
    public LoginCommandAuthorizer()
    {
    }

    public Task<AuthorizationResult> AuthorizeAsync(LoginCommand instance, CancellationToken cancellation = default)
    {
      return Task.FromResult(AuthorizationResult.Success());
    }
  }
}