using AsrTool.Dtos;
using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.MediatR.Businesses.Account.Command;
using AsrTool.Infrastructure.MediatR.Businesses.Account.Queries;
using AsrTool.Infrastructure.MediatR.Businesses.Admin.Command;
using AsrTool.Swagger.Account;
using AsrTool.Swagger.Admin;
using AsrTool.Swagger.Employee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace AsrTool.Controllers
{
  [Authorize]
  public class EmployeeController : BaseApiController
  {
    public EmployeeController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Get user accounts in system
    /// </summary>
    /// <returns>List of users' accounts</returns>
    /// <response code="401">Unauthorized: You are not Logged as employee</response>
    [SwaggerResponseExample(200, typeof(ListAccountDtoExample))]
    [HttpGet("user")]
    public async Task<List<AccountDto>> GetAccounts()
    {
      return await Mediator.Send(new GetAccountsQuery());
    }

    /// <summary>
    /// Create user bank account
    /// </summary>
    /// <returns>Bank account</returns>
    /// <response code="401">Unauthorized: You are not Logged as employee</response>
    [SwaggerResponseExample(200, typeof(BankAccountExample))]
    [HttpPost("create-bank-account")]
    public async Task<BankAccountDto> CreateBankAccount([FromBody] CreateAccountDto request)
    {
      return await Mediator.Send(new CreateAccountCommand { Request = request });
    }

    /// <summary>
    /// Charge money to a user's bank account
    /// </summary>
    /// <returns>Status code</returns>
    /// <response code="401">Unauthorized: You are not Logged as employee</response>
    [SwaggerResponseExample(200, typeof(ChargeMoneyToAccountDtoExample))]
    [HttpPost("charge-money-to-account")]
    public async Task ChaegeMoneyToAccount([FromBody] ChargeMoneyToAccountDto dto)
    {
      await Mediator.Send(new ChargeMoneyToAccountCommand() { Request = dto });
    }

    /// <summary>
    /// Make transaction for user
    /// </summary>
    /// <returns>Status code</returns>
    /// <response code="401">Unauthorized: You are not Logged as employee</response>
    [SwaggerRequestExample(typeof(MakeTransactionDto), typeof(MakeTransactionDtoExample))]
    [HttpPost("make-transaction")]
    public async Task MakeTransaction([FromBody] MakeTransactionDto dto)
    {
      await Mediator.Send(new MakeTransactionCommand() { MakeTransactionDto = dto });
    }

    /// <summary>
    /// Get transactions list of a user
    /// </summary>
    /// <returns>List of transactions</returns>
    /// <response code="401">Unauthorized: You are not Logged as employee</response>
    [SwaggerResponseExample(200, typeof(TransactionExample.ListAccountTransactionExample))]
    [HttpGet("user/{id}/transactions")]
    public async Task<List<TransactionDto>> GetTransaction([FromRoute] int id)
    {
      return await Mediator.Send(new GetTransactionQuery() { AccountId = id });
    }

    /// <summary>
    /// Set user's active status
    /// </summary>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized: You are not Logged as employee</response>
    [SwaggerRequestExample(typeof(SetActiveStatusDto), typeof(SetActiveStatusDtoExample))]
    [HttpPost("set-user-active-status")]
    public async Task SetActiveStatus([FromBody] SetActiveStatusDto request)
    {
      await Mediator.Send(new SetUserActiveStatusCommand { Request = request });
    }
  }
}
