using AsrTool.Dtos;
using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.MediatR.Businesses.Account.Command;
using AsrTool.Infrastructure.MediatR.Businesses.Role.Commands;
using AsrTool.Infrastructure.MediatR.Businesses.User.Commands;
using AsrTool.Infrastructure.MediatR.Businesses.User.Queries;
using AsrTool.Infrastructure.MediatR.Businesses.Admin.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AsrTool.Infrastructure.MediatR.Businesses.Admin.Queries;

namespace AsrTool.Controllers
{
  [Authorize]
  public class AdminController : BaseApiController
  {
    public AdminController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("employee")]
    public async Task<ICollection<EmployeeDto>> GetEmployees()
    {
      return await Mediator.Send(new GetEmployeeQuery());
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
    public async Task<BankAccountDto> CreateBankAccount([FromBody] CreateAccountDto request)
    {
      return await Mediator.Send(new CreateAccountCommand { Request = request });
    }


    [HttpPost("create-employee-account")]
    public async Task<BankAccountDto> CreateEmployeeAccount([FromBody] CreateAccountDto request)
    {
      return await Mediator.Send(new CreateEmployeeAccountCommand { Request = request });
    }

    [HttpPost("set-user-active-status")]
    public async Task SetActiveStatus([FromBody] SetActiveStatusDto request)
    {
      await Mediator.Send(new SetUserActiveStatusCommand { Request = request });
    }


    [HttpPost("list-transactions")]
    public async Task<AdminListTransactionDto> ListTransactions([FromBody] ListTransactionFilter filter)
    {
      return await Mediator.Send(new ListTransactionQuery { Filter = filter });
    }
  }
}
