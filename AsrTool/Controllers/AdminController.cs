using AsrTool.Dtos;
using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.MediatR.Businesses.Account.Command;
using AsrTool.Infrastructure.MediatR.Businesses.Role.Commands;
using AsrTool.Infrastructure.MediatR.Businesses.User.Commands;
using AsrTool.Infrastructure.MediatR.Businesses.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsrTool.Controllers
{
  public class AdminController : BaseApiController
  {
    public AdminController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("getUsersByRoles")]
    public async Task<DataSourceResultDto<UserByRoleDto>> GetUsersByRoles([FromBody] GetUsersByRolesFilterDto request)
    {
      return await Mediator.Send(new GetUsersByRolesQuery()
      {
        RoleIds = request.RoleIds,
        DataSourceRequest = request.Request
      });
    }

    [HttpPost("assignUsersToRole")]
    public async Task AssignUsersToRole([FromBody] AssignUsersToRoleRequestDto request)
    {
      await Mediator.Send(new AssignUsersToRoleCommand()
      {
        Request = request
      });
    }

    [HttpPost("resetRoles")]
    public async Task ResetRoles([FromBody] ResetRolesRequestDto request)
    {
      await Mediator.Send(new ResetRolesCommand()
      {
        Request = request
      });
    }

    [HttpPost("register-admin")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public async Task<bool> Register([FromBody] RegisterRequestDto request)
    {
      await Mediator.Send(new RegisterAdminCommand()
      {
        model = request,
        HttpContext = HttpContext
      });

      return true;
    }


    [HttpPost("create-bank-account")]
    public async Task<BankAccountDto> CreateBankAccount([FromBody] CreateAccountDto request) {
      return await Mediator.Send(new CreateAccountCommand { Request = request });
    }
  }
}
