using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Admin.Command
{
  public class SetUserActiveStatusCommandAuthorizer : IAuthorizer<SetUserActiveStatusCommand>
  {
    private readonly IUserResolver _userResolver;

    public SetUserActiveStatusCommandAuthorizer(IUserResolver userResolver)
    {
      _userResolver = userResolver;
    }


    public async Task<AuthorizationResult> AuthorizeAsync(SetUserActiveStatusCommand instance, CancellationToken cancellation = default)
    {
      if (_userResolver.CurrentUser.RoleName == Constants.Roles.Admin || _userResolver.CurrentUser.RoleName == Constants.Roles.Employee)
      {
        return AuthorizationResult.Success();
      }

      else return AuthorizationResult.Failed();
    }
  }
}
