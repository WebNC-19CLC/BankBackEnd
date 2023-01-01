using MediatR;
using AsrTool.Dtos;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class CreateDebitCommand : IRequest<DebitDto>
  {
    public CreateDebitDto Request;
  }
}
