using AsrTool.Dtos;
using AsrTool.Infrastructure.MediatR.Businesses.Role.Commands;
using AsrTool.Infrastructure.MediatR.Businesses.Role.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsrTool.Controllers
{ 
  public class RoleController : BaseApiController
  {
    public RoleController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("getRolesByNames")]
    public async Task<IEnumerable<RoleDto>> GetRolesByNames([FromBody] string[] names)
    {
      return await Mediator.Send(new GetRolesByNamesQuery()
      {
        Names = names
      });
    }

    [HttpGet("{id}")]
    public async Task<RoleDto> Get([FromRoute] int id)
    {
      return await Mediator.Send(new GetRoleQuery() { RoleId = id });
    }

    [HttpPost]
    public async Task Create([FromBody] RoleDto roleDto)
    {
      await Mediator.Send(new CreateRoleCommand() { Role = roleDto });
    }

    [HttpPut]
    public async Task Update([FromBody] RoleDto roleDto)
    {
      await Mediator.Send(new UpdateRoleCommand() { Role = roleDto });
    }

    [HttpDelete("{id}")]
    public async Task Delete([FromRoute] int id)
    {
      await Mediator.Send(new DeleteRoleCommand() { RoleId = id });
    }
  }
}
