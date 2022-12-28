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
      var users = _asrContext.Get<Domain.Entities.Employee>().Include(x => x.Role).Include(x => x.BankAccount).Where(x => x.BankAccountId != null);
      return await users.Select(x => new AccountDto
      {
        BankAccountId = x.BankAccount.Id,
        IsActive = x.Active,
        AccountNumber = x.BankAccount.AccountNumber,
        Email = x.Email,
        FullName = x.FullName,
        Balance = x.BankAccount.Balance,
        Role = x.Role.Name,
      }).ToListAsync();
    }
  }
}
