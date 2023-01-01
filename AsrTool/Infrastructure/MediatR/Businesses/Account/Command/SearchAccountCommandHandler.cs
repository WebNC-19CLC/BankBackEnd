using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class SearchAccountCommandHandler : IRequestHandler<SearchAccountCommand, ShortAccountDto>
  {
    private readonly IAsrContext _asrContext;
    private readonly IUserResolver _userResolver;
    private readonly IMediator _mediator;

    public SearchAccountCommandHandler(IAsrContext asrContext, IUserResolver userResolver, IMediator mediator)
    {
      _asrContext = asrContext;
      _userResolver = userResolver;
      _mediator = mediator;
    }

    public async Task<ShortAccountDto?> Handle(SearchAccountCommand request, CancellationToken cancellationToken)
    {
      var bankAccount = await _asrContext.Get<BankAccount>().SingleOrDefaultAsync(x => x.UserId == _userResolver.CurrentUser.Id);

      var searchUser = await _asrContext.Get<Employee>().Include(x => x.BankAccount).SingleOrDefaultAsync(x => x.BankAccount.AccountNumber == request.AccountNumber && x.BankAccount.AccountNumber != bankAccount.AccountNumber);

      if (searchUser == null)
      {
        return null;
      }
      else
      {
        return new ShortAccountDto
        {
          BankAccountId = searchUser.BankAccount.Id,
          AccountNumber = searchUser.BankAccount.AccountNumber,
          FullName = searchUser.FullName
        };
      }
    }
  }
}
