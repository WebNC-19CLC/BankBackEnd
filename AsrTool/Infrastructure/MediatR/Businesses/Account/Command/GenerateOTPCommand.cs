using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class GenerateOTPCommand : IRequest
  {
    public string? Username { get; set; } = null;
  }
}
