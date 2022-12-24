using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.Common.Grid;
using AsrTool.Infrastructure.Domain.Entities;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Queries
{
  public class GetUsersByRolesQuery : GridQuery<Employee, UserByRoleDto>
  {
    public ICollection<int> RoleIds { get; set; }
  }
}
