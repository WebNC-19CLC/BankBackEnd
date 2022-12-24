using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Commands
{
  public class DeleteRoleCommand : IRequest
  {
    public int RoleId { get; set; }
  }
}