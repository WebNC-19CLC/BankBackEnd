using AsrTool.Dtos;
using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.MediatR.Businesses.Role.Commands;
using AsrTool.Infrastructure.MediatR.Businesses.Account.Command;
using AsrTool.Infrastructure.MediatR.Businesses.Account.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AsrTool.Swagger.Admin;
using Swashbuckle.AspNetCore.Filters;
using AsrTool.Swagger.Account;
using AsrTool.Infrastructure.Domain.Entities;

namespace AsrTool.Controllers
{
  [Authorize]
  public class AccountController : BaseApiController
  {
    public AccountController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Get current logged in customer's bank account 
    /// </summary>
    /// <returns>A Customer Bank Account</returns>
    /// <response code="401">Unauthorized: You are not Logged</response>
    [SwaggerResponseExample(200, typeof(BankAccountExample))]
    [HttpGet("me")]
    public async Task<AccountDto> GetCurrentAccount()
    {
      return await Mediator.Send(new GetCurrentAccountQuery()
      {

      });
    }

    /// <summary>
    /// Get an recipient based on bank account number 
    /// </summary>
    /// <returns>A Revcipient</returns>
    /// <response code="401">Unauthorized: You are not Logged</response>
    [SwaggerRequestExample(typeof(GetRecipientRequestDto), typeof(RecipientExample.GetRecipientRequestDtoExample))]
    [SwaggerResponseExample(200, typeof(RecipientExample.CreateRecipientResponseExample))]
    [HttpPost("get-recipient")]
    public async Task<RecipientDto> GetRecipient([FromBody] GetRecipientRequestDto dto)
    {
      return await Mediator.Send(new GetRecipientQuery() { Request = dto });
    }

    /// <summary>
    /// Get current logged in user's list transaction
    /// </summary>
    /// <returns>A list of Employees</returns>
    /// <response code="401">Unauthorized: You are not Logged</response>
    [SwaggerResponseExample(200, typeof(TransactionExample.ListAccountTransactionExample))]
    [HttpGet("me/transactions")]
    public async Task<ICollection<TransactionDto>> GetMyTransactions()
    {
      return await Mediator.Send(new GetMyTransactionsQuery());
    }

    /// <summary>
    /// Get current logged in user's list recipient
    /// </summary>
    /// <returns>A list of recipients</returns>
    /// <response code="401">Unauthorized: You are not Logged</response>
    [SwaggerResponseExample(200, typeof(RecipientExample.ListRecipientExample))]
    [HttpGet("me/my-recipients")]
    public async Task<ICollection<RecipientDto>> GetMyRecipients()
    {
      return await Mediator.Send(new GetMyRecipientsQuery());
    }

    /// <summary>
    /// Delete account's recipient by id
    /// </summary>
    /// <returns>Status code</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Bad request: Failed to valid Request Params</response>
    /// <response code="401">Unauthorized: You are not Logged</response>
    /// <response code="406">Bussiness reason with message</response>
    [HttpDelete("me/my-recipients/{id}")]
    public async Task DeleteMyRecipients([FromRoute] int id)
    {
      await Mediator.Send(new DeleteMyRecipientCommand() { Id = id });
    }

    /// <summary>
    /// Edit account's recipient by id
    /// </summary>
    /// <returns>Edited recipient</returns>
    /// <response code="400">Bad request: Failed to valid Request Body</response>
    /// <response code="401">Unauthorized: You are not Logged</response>
    /// <response code="406">Bussiness reason with message</response>
    [SwaggerResponseExample(200, typeof(RecipientExample))]
    [SwaggerRequestExample(typeof(RecipientDto), typeof(RecipientExample))]
    [HttpPost("me/edit-my-recipient")]
    public async Task<RecipientDto> EditMyRecipient([FromBody] RecipientDto editDto)
    {
      return await Mediator.Send(new EditMyRecipientCommand() { Request = editDto });
    }

    /// <summary>
    /// Create recipient
    /// </summary>
    /// <returns>Added recipient</returns>
    /// <response code="400">Bad request: Failed to valid request body</response>
    /// <response code="401">Unauthorized: You are not Logged</response>
    /// <response code="406">Bussiness reason with message</response>
    [SwaggerResponseExample(200, typeof(RecipientExample.CreateRecipientResponseExample))]
    [SwaggerRequestExample(typeof(CreateRecipientDto), typeof(RecipientExample.CreateRecipientRequestExample))]
    [HttpPost("me/add-my-recipient")]
    public async Task<RecipientDto> AddRecipient([FromBody] CreateRecipientDto dto)
    {
      return await Mediator.Send(new AddMyRecipientCommand() { Request = dto });
    }

    /// <summary>
    /// Transfer moeny to account
    /// </summary>
    /// <returns>Status code</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Bad request: Failed to valid request body</response>
    /// <response code="406">Bussiness reason with message</response>
    [SwaggerRequestExample(typeof(SelfTransferDto), typeof(TransactionExample.TranferMoney))]
    [HttpPost("me/transfer-money")]
    public async Task TransferMoney([FromBody] SelfTransferDto dto)
    {
      await Mediator.Send(new TransferMoneyCommand() { Request = dto });
    }


    /// <summary>
    /// Change account password
    /// </summary>
    /// <returns>Status code</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Bad request: Failed to valid request body</response>
    /// <response code="401">Unauthorized: You are not Logged</response>
    /// <response code="406">Bussiness reason with message</response>
    [SwaggerRequestExample(typeof(ChangePasswordDto), typeof(AccountExample.ChangePasswordRequestExample))]
    [HttpPost("me/change-password")]
    public async Task ChangePassword([FromBody] ChangePasswordDto dto)
    {
      await Mediator.Send(new ChangeMyPasswordCommand() { Request = dto });
    }

    /// <summary>
    /// Get list account's debits
    /// </summary>
    /// <returns>List Logged in account's debits</returns>
    /// <response code="401">Unauthorized: You are not Logged</response>
    [SwaggerResponseExample(200, typeof(DebitExample.GetListDebitResponseExample))]
    [HttpGet("me/debits")]
    public async Task<ICollection<DebitDto>> GetDebits()
    {
      return await Mediator.Send(new GetMyDebitsQuery());
    }

    /// <summary>
    /// Cancel specific account's debit
    /// </summary>
    /// <returns>Status code</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Bad request: Failed to valid request body</response>
    /// <response code="401">Unauthorized: You are not Logged</response>
    /// <response code="406">Bussiness reason with message</response>
    [SwaggerRequestExample(typeof(DeleteDebitRequestDto), typeof(DebitExample.CancelDebitRequest))]
    [HttpPost("me/debits/cancel")]
    public async Task DeleteMyDebit([FromBody] DeleteDebitRequestDto dto)
    {
      await Mediator.Send(new DeleteMyDebitCommand() { Request = dto });
    }

    /// <summary>
    /// Create Debit
    /// </summary>
    /// <returns>Debit</returns>
    /// <response code="400">Bad request: Failed to valid request body</response>
    /// <response code="401">Unauthorized: You are not Logged</response>
    /// <response code="406">Bussiness reason with message</response>
    [SwaggerRequestExample(typeof(CreateDebitDto), typeof(DebitExample.CreateDebitRequest))]
    [SwaggerResponseExample(200, typeof(DebitExample))]
    [HttpPost("me/debits")]
    public async Task<DebitDto> AddNewDebit([FromBody] CreateDebitDto dto)
    {
      return await Mediator.Send(new CreateDebitCommand() { Request = dto });
    }


    /// <summary>
    /// Pay Debit account's debit by id
    /// </summary>
    /// <returns>Status code</returns>
    /// <response code="200">Success</response> 
    /// <response code="400">Bad request: Failed to valid request body</response>
    /// <response code="401">Unauthorized: You are not Logged</response>
    /// <response code="406">Bussiness reason with message</response>
    [SwaggerRequestExample(typeof(PayDebitDto), typeof(DebitExample.PayDebitRequest))]
    [HttpPost("me/debits/pay")]
    public async Task AddNewDebit([FromBody] PayDebitDto dto)
    {
      await Mediator.Send(new PayMyDebitCommand() { Id = dto.Id, OTP = dto.OTP });
    }

    /// <summary>
    /// Search Account
    /// </summary>
    /// <returns>Short Info Bank Account</returns>
    /// <response code="400">Bad request: Failed to valid request body</response>
    /// <response code="401">Unauthorized: You are not Logged</response>
    /// <response code="406">Bussiness reason with message</response>
    [SwaggerResponseExample(200, typeof(BankAccountExample.SearchAccountResponse))]
    [SwaggerRequestExample(typeof(SearchAccountRequestDto), typeof(BankAccountExample.SearchAccountRequest))]
    [HttpPost("me/searchAccount")]
    public async Task<ShortAccountDto?> SearchAccount([FromBody] SearchAccountRequestDto dto)
    {
      return await Mediator.Send(new SearchAccountCommand() { AccountNumber = dto.AccountNumber, bankId = dto.BankId });
    }

    /// <summary>
    /// Generate Otp in Email
    /// </summary>
    /// <returns>Status code</returns>
    /// <response code="200">Success</response> 
    /// <response code="401">Unauthorized: You are not Logged</response>
    [HttpPost("me/generate-otp")]
    public async Task GenerateOTP()
    {
      await Mediator.Send(new GenerateOTPCommand() { });
    }

    /// <summary>
    /// Get All Notification
    /// </summary>
    /// <returns>A Customer Bank Account</returns>
    /// <response code="401">Unauthorized: You are not Logged</response>
    [SwaggerResponseExample(200, typeof(NotificationExample.GetListNotificationExample))]
    [HttpGet("me/notifications")]
    public async Task<ICollection<NotifationDto>> GetMyNotifications()
    {
      return await Mediator.Send(new GetMyNotificationsQuery());
    }
  }
}
