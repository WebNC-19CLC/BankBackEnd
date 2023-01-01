using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class SendOTPEmailCommand : IRequest
  {
    public string Email { get; set; }
    public string OTP { get; set; }
  }
}
