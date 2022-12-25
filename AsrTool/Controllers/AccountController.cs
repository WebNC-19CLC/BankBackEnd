using AsrTool.Dtos;
using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.MediatR.Businesses.Role.Commands;
using AsrTool.Infrastructure.MediatR.Businesses.Account.Command;
using AsrTool.Infrastructure.MediatR.Businesses.Account.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsrTool.Controllers
{
  public class AccountController : BaseApiController
  {
    public AccountController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<List<AccountDto>> GetAccounts() {
      return await Mediator.Send(new GetAccountsQuery());
    }



    [HttpGet("{id}/transaction")]
    public async Task<List<TransactionDto>> Get([FromRoute] int id)
    {
      return await Mediator.Send(new GetTransactionQuery() { AccountId = id });
    }

    [HttpPost("make-transaction")]
    public async Task MakeTransaction([FromBody] MakeTransactionDto dto) {
      await Mediator.Send(new MakeTransactionCommand() { MakeTransactionDto = dto });
    }
  }
}
