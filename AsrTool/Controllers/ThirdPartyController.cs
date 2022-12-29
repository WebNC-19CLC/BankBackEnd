using AsrTool.Dtos;
using AsrTool.Infrastructure.MediatR.Businesses.ThirdParty.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AsrTool.Controllers
{
    public class ThirdPartyController : BaseApiController
    {
        public ThirdPartyController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("customers/{AccountNumber}")]
        [Authorize(Policy = "PRE:ThirdPartyReadApiPolicy")]
        public async Task<AccountDto> GetCustomerByAccountNumber([FromRoute] string accountNumber)
        {
            var result = await Mediator.Send(new GetCustomerInfoQuery
            {
                AccountNumber = accountNumber
            });
            return result;
        }

        [HttpPost("transactions")]
        [Authorize(Policy = "PRE:ThirdPartyTransactionApiPolicy")]
        public async Task<List<AccountDto>> MakeTransaction([FromBody]  SelfReceiveDto selfReceiveDto)
        {
            return null;
        }

    }
}
