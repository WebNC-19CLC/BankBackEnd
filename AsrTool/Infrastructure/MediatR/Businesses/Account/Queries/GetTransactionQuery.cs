using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Queries
{
  public class GetTransactionQuery : IRequest<List<TransactionDto>>
  {
    public int AccountId { get; set; } 
  }
}
