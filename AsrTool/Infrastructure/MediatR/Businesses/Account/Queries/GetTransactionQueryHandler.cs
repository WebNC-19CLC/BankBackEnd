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
      return await _asrContext.Get<Transaction>().Include(x => x.From)
        .Include(x => x.To)
        .Where(x => x.FromId == request.AccountId || x.ToId == request.AccountId )
        .Select(x => new TransactionDto { Id = x.Id, From = x.From.AccountNumber, To = x.To.AccountNumber, Amount = x.Amount , Time = x.CreatedOn })
        .ToListAsync();
    }
  }
}
