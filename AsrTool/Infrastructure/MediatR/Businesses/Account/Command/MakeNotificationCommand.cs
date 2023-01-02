using MediatR;
using AsrTool.Dtos;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class MakeNotificationCommand : IRequest
  {
    public MakeNotificationDto Request { get; set; }
  }
}
