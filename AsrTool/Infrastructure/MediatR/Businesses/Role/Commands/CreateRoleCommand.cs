using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Commands
{
  public class CreateRoleCommand : IRequest
  {
    public RoleDto Role { get; set; }
  }
}