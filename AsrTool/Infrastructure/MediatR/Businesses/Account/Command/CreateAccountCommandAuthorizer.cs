using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class CreateAccountCommandAuthorizer : IAuthorizer<CreateAccountCommand>
  {
    private readonly IUserResolver _userResolver;

    public CreateAccountCommandAuthorizer(IUserResolver userResolver)
    {
      _userResolver = userResolver;
    }

    public async Task<AuthorizationResult> AuthorizeAsync(CreateAccountCommand instance, CancellationToken cancellation = default)
    {
      if (_userResolver.CurrentUser.RoleName == Constants.Roles.Admin)
      {
        return AuthorizationResult.Success();
      }
      else return AuthorizationResult.Failed();
    }
  }
}
