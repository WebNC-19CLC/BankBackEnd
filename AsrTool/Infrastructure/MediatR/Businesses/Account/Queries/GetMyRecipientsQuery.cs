using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Queries
{
  public class GetMyRecipientsQuery : IRequest<ICollection<RecipientDto>>
  {

  }
}
