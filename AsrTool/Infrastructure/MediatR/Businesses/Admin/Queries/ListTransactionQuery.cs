using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Admin.Queries
{
  public class ListTransactionQuery : IRequest<ICollection<TransactionDto>>
  {
    public ListTransactionFilter Filter { get; set; }
  }
}
