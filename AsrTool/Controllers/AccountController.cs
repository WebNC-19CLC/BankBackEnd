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

    [HttpGet("me")]
    public async Task<AccountDto> GetCurrentAccount()
    {
      return await Mediator.Send(new GetCurrentAccountQuery()
      {

      });
    }

    [HttpPost("get-recipient")]
    public async Task<RecipientDto> GetRecipient([FromBody] GetRecipientRequestDto dto)
    {
      return await Mediator.Send(new GetRecipientQuery() { Request = dto });
    }

    [HttpGet("me/transactions")]
    public async Task<ICollection<TransactionDto>> GetMyTransactions()
    {
      return await Mediator.Send(new GetMyTransactionsQuery());
    }

    [HttpGet("me/my-recipients")]
    public async Task<ICollection<RecipientDto>> GetMyRecipients()
    {
      return await Mediator.Send(new GetMyRecipientsQuery());
    }

    [HttpDelete("me/my-recipients/{id}")]
    public async Task DeleteMyRecipients([FromRoute] int id)
    {
      await Mediator.Send(new DeleteMyRecipientCommand() { Id = id });
    }


    [HttpPost("me/edit-my-recipient")]
    public async Task<RecipientDto> EditMyRecipient([FromBody] RecipientDto editDto)
    {
      return await Mediator.Send(new EditMyRecipientCommand() { Request = editDto });
    }

    [HttpPost("me/add-my-recipient")]
    public async Task<RecipientDto> AddRecipient([FromBody] CreateRecipientDto dto)
    {
      return await Mediator.Send(new AddMyRecipientCommand() { Request = dto });
    }

    [HttpPost("me/transfer-money")]
    public async Task TransferMoney([FromBody] SelfTransferDto dto)
    {
      await Mediator.Send(new TransferMoneyCommand() { Request = dto });
    }

    [HttpPost("/me/change-password")]
    public async Task ChangePassword([FromBody] ChangePasswordDto dto)
    {
      await Mediator.Send(new ChangeMyPasswordCommand() { Request = dto });
    }
  }
}
