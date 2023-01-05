using MediatR;
using AsrTool.Dtos;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class DeleteMyDebitCommand : IRequest
  {
    public DeleteDebitRequestDto Request; 
  }
}
