using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Queries
{
  public class GetTransactionQueryHandler : IRequestHandler<GetTransactionQuery, List<TransactionDto>>
  {
    private readonly IAsrContext _asrContext;

    public GetTransactionQueryHandler(IAsrContext asrContext)
    {
      _asrContext = asrContext;
    }
    public async Task<List<TransactionDto>> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
    {
      return await _asrContext.Get<Transaction>().Include(x => x.From).ThenInclude(x => x.User)
        .Include(x => x.To).ThenInclude(x => x.User)
        .Where(x => x.FromId == request.AccountId || x.ToId == request.AccountId )
        .OrderByDescending(x => x.Id)
        .Select(x => new TransactionDto { 
          Id = x.Id, 
          FromAccountNumber = x.From.AccountNumber, 
          ToAccountNumber = x.To.AccountNumber, 
          Amount = x.Amount , 
          Time = x.CreatedOn,
          FromUser = x.From.User.FullName,
          ToUser = x.To.User.FullName,
          BankDestinationId = x.BankDestinationId,
          BankSourceId = x.BankSourceId,
        })
        .ToListAsync();
    }
  }
}
