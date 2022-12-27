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
  [Authorize]
  public class AccountController : BaseApiController
  {
    public AccountController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<List<AccountDto>> GetAccounts()
    {
      return await Mediator.Send(new GetAccountsQuery());
    }

    [HttpGet("{id}/transaction")]
    public async Task<List<TransactionDto>> GetTransaction([FromRoute] int id)
    {
      return await Mediator.Send(new GetTransactionQuery() { AccountId = id });
    }

    [HttpGet("me")]
    public async Task<AccountDto> GetCurrentAccount()
    {
      return await Mediator.Send(new GetCurrentAccountQuery()
      {

      });
    }

    [HttpPost("get-recipient")]
    public async Task<RecipientDto> GetRecipient([FromBody] GetRecipientRequestDto dto) {
      return await Mediator.Send(new GetRecipientQuery() { Request = dto });
    }

    [HttpGet("me/my-recipients")]
    public async Task<ICollection<RecipientDto>> GetMyRecipients() {
      return await Mediator.Send(new GetMyRecipientsQuery());
    }

    [HttpPost("me/edit-my-recipient")]
    public async Task<RecipientDto> EditMyRecipient([FromBody] RecipientDto editDto) {
      return await Mediator.Send(new EditMyRecipientCommand() {Request = editDto });
    }

    [HttpPost("me/add-my-recipient")]
    public async Task<RecipientDto> AddRecipient([FromBody] CreateRecipientDto dto)
    {
      return await Mediator.Send(new AddMyRecipientCommand() { Request = dto });
    }

    [HttpPost("make-transaction")]
    public async Task MakeTransaction([FromBody] MakeTransactionDto dto)
    {
      await Mediator.Send(new MakeTransactionCommand() { MakeTransactionDto = dto });
    }
  }
}
