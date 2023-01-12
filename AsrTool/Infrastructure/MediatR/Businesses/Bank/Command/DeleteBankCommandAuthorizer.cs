using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.MediatR.Businesses.Bank.Command;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace AsrTool.Infrastructure.MediatR.Businesses.Admin.Queries
{
  public class DeleteBankCommandAuthorizer : IAuthorizer<DeleteBankCommand>
  {
    private readonly IUserResolver _userResolver;

    public DeleteBankCommandAuthorizer(IUserResolver userResolver)
    {
      _userResolver = userResolver;
    }

    public async Task<AuthorizationResult> AuthorizeAsync(DeleteBankCommand instance, CancellationToken cancellation = default)
    {
      if (_userResolver.CurrentUser.RoleName == Constants.Roles.Admin)
      {
        return AuthorizationResult.Success();
      }

      else return AuthorizationResult.Failed();
    }
  }
}
