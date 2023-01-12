using AsrTool.Dtos;
using AsrTool.Infrastructure.Filters;
using AsrTool.Infrastructure.MediatR.Businesses.ThirdParty.Command;
using AsrTool.Infrastructure.MediatR.Businesses.ThirdParty.Queries;
using AsrTool.Swagger.Account;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Swashbuckle.AspNetCore.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AsrTool.Controllers
{
    /// <summary>
    /// API are for associated banks to call, client apps do not call these api
    /// </summary>
    public class ThirdPartyController : BaseApiController
    {
        public ThirdPartyController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get one customer info
        /// </summary>
        /// <returns>Return Object</returns>
        /// 
        /// <response code ="200">
        /// Return Object {<br/>
        ///     BankAccountId: int?, <br/>
        ///     AccountNumber: string?, 
        ///     Fullname: string <br/>
        ///     }
        /// </response>
        /// 
        /// <response code="400">Bad request: Failed to valid request body</response>
        /// <response code="401">Unauthorized: Authorize fail with message</response>
        /// <response code="406">Bussiness reason with message</response>
        [HttpGet("customers/{AccountNumber}")]
        //[Authorize(Policy = "PRE:ThirdPartyReadApiPolicy")]
        public async Task<ShortAccountDto> GetCustomerByAccountNumber([FromRoute] string AccountNumber)
        {
            var result = await Mediator.Send(new GetCustomerInfoQuery
            {
                AccountNumber = AccountNumber
            });
            return result;
        }

        /// <summary>
        /// Make one transaction but do not deduce balance customer's account
        ///  - 
        /// response signature in header [XApikey]
        /// </summary>
        /// <returns>Return Object</returns>
        /// <response code ="200">
        /// Return Object {<br/>
        ///     Id: int?, <br/>
        ///     FromAccountNumber: string, 
        ///     ToAccountNumber: string <br/>
        ///     FromUser: string? <br/>
        ///     ToUser: string? <br/>
        ///     Type: string <br/>
        ///     BankDestinationId: int? <br/>
        ///     BankSourceId: int? <br/>
        ///     BankSourceId: int? <br/>
        ///     Amount: Double? <br/>
        ///     Time: DateTime <br/>
        ///     }
        /// </response>
        /// 
        /// <response code="400">Bad request: Failed to valid request body</response>
        /// <response code="401">Unauthorized: Authorize fail with message</response>
        /// <response code="406">Bussiness reason with message</response>
        [SwaggerRequestExample(typeof(MakeTransactionDto),typeof(TransactionExample))]
        [HttpPost("transactions")]
        //[Authorize(Policy = "PRE:ThirdPartyTransactionApiPolicy")]
        public async Task<TransactionDto> MakeTransaction([FromBody]  MakeTransactionDto makeTransactionDto,
            [FromHeader(Name = "BankSourceId")] string bankSourceId)
        {
            var result = await Mediator.Send(new ReceiveTransactionCommand
            {
                makeTransaction = makeTransactionDto,
                bankSourceId = int.Parse(bankSourceId),
            });
            return result;
        }


        /// <summary>
        /// Complete transaction: Deduce money from customer account
        ///  - 
        /// Call this api after sucess to valid response signuate, 
        /// </summary>
        /// <returns>Status code</returns>
        /// <response code ="200">Sucess</response>
        /// <response code="400">Bad request: Failed to valid request body</response>
        /// <response code="401">Unauthorized: Authorize fail with message</response>
        /// <response code="406">Bussiness reason with message</response>
        [HttpPut("transactions/{transactionId}")]
        //[Authorize(Policy = "PRE:ThirdPartyTransactionApiPolicy")]
        public async Task<TransactionDto> CompleteTransaction([FromRoute] int transactionId)
        {
            var result = await Mediator.Send(new CompleteTransactionCommand
            {
                Id = transactionId
            });
            return result;
        }

    }
}
