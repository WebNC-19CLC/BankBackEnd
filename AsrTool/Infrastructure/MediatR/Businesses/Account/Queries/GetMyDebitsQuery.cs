using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Queries
{
  public class GetMyDebitsQuery : IRequest<ICollection<DebitDto>>
  {
  }
}
