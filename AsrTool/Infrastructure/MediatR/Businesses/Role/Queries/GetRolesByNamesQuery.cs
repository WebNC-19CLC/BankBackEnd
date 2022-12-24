using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Queries
{
  public class GetRolesByNamesQuery : IRequest<IEnumerable<RoleDto>>
  {
    public string[] Names { get; set; }
  }
}
