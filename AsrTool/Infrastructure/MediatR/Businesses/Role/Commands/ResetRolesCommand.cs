using AsrTool.Dtos.UserRoleDtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Commands
{
  public class ResetRolesCommand : IRequest
  {
    public ResetRolesRequestDto Request { get; set; }
  }
}
