using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class ChangeMyPasswordCommand : IRequest
  {
    public ChangePasswordDto Request { get; set; }
  }
}
