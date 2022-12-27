using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Queries
{
  public class GetRecipientQuery : IRequest<RecipientDto>
  {
    public GetRecipientRequestDto Request {get; set;}
  }
}
