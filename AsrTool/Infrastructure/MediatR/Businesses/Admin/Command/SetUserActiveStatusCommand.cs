using MediatR;
using AsrTool.Dtos;

namespace AsrTool.Infrastructure.MediatR.Businesses.Admin.Command
{
  public class SetUserActiveStatusCommand : IRequest
  {
    public SetActiveStatusDto Request { get; set; }
  }
}
