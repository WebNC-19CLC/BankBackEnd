using MediatR;
using AsrTool.Dtos;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class ForgotPasswordCommand : IRequest
  {
    public ForgotPasswordDto Request;
  }
}
