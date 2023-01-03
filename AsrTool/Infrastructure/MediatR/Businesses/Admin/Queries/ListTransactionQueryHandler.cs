using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using AsrTool.Infrastructure.Auth;

namespace AsrTool.Infrastructure.MediatR.Businesses.Admin.Queries
{
  public class ListTransactionQueryHandler : IRequestHandler<ListTransactionQuery, ICollection<TransactionDto>>
  {
    private readonly IAsrContext _asrContext;

    public ListTransactionQueryHandler(IAsrContext asrContext)
    {
      _asrContext = asrContext;
    }

    public async Task<ICollection<TransactionDto>> Handle(ListTransactionQuery request, CancellationToken cancellationToken)
    {
      var query = _asrContext.Get<Transaction>().Include(x => x.From).ThenInclude(x => x.User)
        .Include(x => x.To).ThenInclude(x => x.User).AsSplitQuery();

      if (request.Filter.DateStart != null) {
        query = query.Where(x => x.CreatedOn >= request.Filter.DateStart);
      }

      if (request.Filter.DateEnd != null)
      {
        query = query.Where(x => x.CreatedOn <= request.Filter.DateEnd);
      }

      if (request.Filter.BankDestinationId != null)
      {
        query = query.Where(x => x.BankDestinationId == request.Filter.BankDestinationId);
      }

      if (request.Filter.BankSourceId != null)
      {
        query = query.Where(x => x.BankSourceId == request.Filter.BankDestinationId);
      }

      return await query.OrderByDescending(x => x.Id).Select(x => new TransactionDto
        {
          Id = x.Id,
          FromAccountNumber = x.From.AccountNumber,
          ToAccountNumber = x.To.AccountNumber,
          Amount = x.Amount,
          Time = x.CreatedOn,
          BankDestinationId = x.BankDestinationId,
          BankSourceId = x.BankSourceId,
          Type = x.Type,
          FromUser = x.From.User.FullName,
          ToUser = x.To.User.FullName
        })
        .ToListAsync();
    }
  }
}
