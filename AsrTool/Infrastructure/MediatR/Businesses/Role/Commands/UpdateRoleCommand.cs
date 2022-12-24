using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Commands
{
  public class UpdateRoleCommand : IRequest
  {
    public RoleDto Role { get; set; }
  }
}