using AsrTool.Dtos.UserRoleDtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Commands
{
  public class AssignUsersToRoleCommand : IRequest
  {
    public AssignUsersToRoleRequestDto Request { get; set; }
  }
}
