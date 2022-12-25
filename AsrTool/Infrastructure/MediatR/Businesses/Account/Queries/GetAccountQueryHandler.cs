using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Queries
{
  public class GetAccountQueryHandler : IRequestHandler<GetAccountsQuery, List<AccountDto>>
  {

    private readonly IAsrContext _asrContext;

    public GetAccountQueryHandler(IAsrContext asrContext)
    {
      _asrContext = asrContext;
    }
    public async Task<List<AccountDto>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
      return await _asrContext.Get<Domain.Entities.BankAccount>().Select(x => new AccountDto { Id = x.Id, AccountNumber = x.AccountNumber, Balance = x.Balance }).ToListAsync();
    }
  }
}
