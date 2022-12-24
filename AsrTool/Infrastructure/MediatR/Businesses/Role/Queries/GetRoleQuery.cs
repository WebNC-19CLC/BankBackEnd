using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Queries
{
  public class GetRoleQuery : IRequest<RoleDto>
  {
    public int RoleId { get; set; }
  }
}