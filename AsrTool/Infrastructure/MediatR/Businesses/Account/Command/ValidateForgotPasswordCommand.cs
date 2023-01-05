using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class ValidateForgotPasswordCommand : IRequest
  {
    public ValidateOTPForgotPasswordDto Request { get; set; }
  }
}
