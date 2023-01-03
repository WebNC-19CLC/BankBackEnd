using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class MakeTransactionCommandAuthorizer : IAuthorizer<MakeTransactionCommand>
  {
    private readonly IUserResolver _userResolver;
    private readonly IAsrContext _asrContext;

    public MakeTransactionCommandAuthorizer(IUserResolver userResolver, IAsrContext asrContext)
    {
      _userResolver = userResolver;
      _asrContext = asrContext;
    }

    public async Task<AuthorizationResult> AuthorizeAsync(MakeTransactionCommand instance, CancellationToken cancellation = default)
    {
      var user = await _asrContext.Get<Employee>().SingleOrDefaultAsync(x => x.Id == _userResolver.CurrentUser.Id);

      if (user?.Active == true)
      {
        return AuthorizationResult.Success();
      }

      else return AuthorizationResult.Failed();
    }
  }
}
