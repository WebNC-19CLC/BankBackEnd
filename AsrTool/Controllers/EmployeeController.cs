using AsrTool.Dtos;
using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.MediatR.Businesses.Account.Command;
using AsrTool.Infrastructure.MediatR.Businesses.Account.Queries;
using AsrTool.Infrastructure.MediatR.Businesses.Admin.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsrTool.Controllers
{
  [Authorize]
  public class EmployeeController : BaseApiController
  {
    public EmployeeController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("user")]
    public async Task<List<AccountDto>> GetAccounts()
    {
      return await Mediator.Send(new GetAccountsQuery());
    }

    [HttpPost("create-bank-account")]
    public async Task<BankAccountDto> CreateBankAccount([FromBody] CreateAccountDto request)
    {
      return await Mediator.Send(new CreateAccountCommand { Request = request });
    }

    [HttpPost("charge-money-to-account")]
    public async Task ChaegeMoneyToAccount([FromBody] ChargeMoneyToAccountDto dto)
    {
      await Mediator.Send(new ChargeMoneyToAccountCommand() { Request = dto });
    }

    [HttpPost("make-transaction")]
    public async Task MakeTransaction([FromBody] MakeTransactionDto dto)
    {
      await Mediator.Send(new MakeTransactionCommand() { MakeTransactionDto = dto });
    }

    [HttpGet("user/{id}/transactions")]
    public async Task<List<TransactionDto>> GetTransaction([FromRoute] int id)
    {
      return await Mediator.Send(new GetTransactionQuery() { AccountId = id });
    }

    [HttpPost("set-user-active-status")]
    public async Task SetActiveStatus([FromBody] SetActiveStatusDto request)
    {
      await Mediator.Send(new SetUserActiveStatusCommand { Request = request });
    }
  }
}
