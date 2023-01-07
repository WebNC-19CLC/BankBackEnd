using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Queries
{
  public class GetMyTransactionsQueryHandler : IRequestHandler<GetMyTransactionsQuery, ICollection<TransactionDto>>
  {
    private readonly IAsrContext _asrContext;
    private readonly IUserResolver _userResolver;

    public GetMyTransactionsQueryHandler(IAsrContext asrContext, IUserResolver userResolver)
    {
      _asrContext = asrContext;
      _userResolver = userResolver;
    }
    public async Task<ICollection<TransactionDto>> Handle(GetMyTransactionsQuery request, CancellationToken cancellationToken)
    {
      var user = await _asrContext.Get<Employee>().Include(x => x.BankAccount).SingleOrDefaultAsync(x => x.Id == _userResolver.CurrentUser.Id);

      if (user.BankAccount == null)
      {
        throw new BusinessException("This user do not have bank account");
      }

      return await _asrContext.Get<Transaction>()
        .Include(x => x.From).ThenInclude(x => x.User)
        .Include(x => x.To).ThenInclude(x => x.User).AsSplitQuery()
        .Where(x => x.FromId == user.BankAccount.Id || x.ToId == user.BankAccount.Id)
        .Where(x => x.CreatedOn >= DateTime.UtcNow.AddDays(-30))
        .OrderByDescending(x => x.Id)
        .Select(x => new TransactionDto {
          Id = x.Id,
          FromAccountNumber = x.FromAccountNumber != null ? x.FromAccountNumber : x.From.AccountNumber,
          ToAccountNumber = x.ToAccountNumber != null ? x.ToAccountNumber : x.To.AccountNumber,
          Amount = x.Amount,
          Time = x.CreatedOn,
          BankDestinationId = x.BankDestinationId,
          BankSourceId = x.BankSourceId,
          Type = x.ToId == user.BankAccount.Id ? "Receive" : x.Type,
          FromUser = x.From.User.FullName,
          ToUser = x.To.User.FullName
        })
        .ToListAsync();
    }
  }
}
