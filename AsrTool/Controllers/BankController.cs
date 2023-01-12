using AsrTool.Dtos;
using AsrTool.Infrastructure.MediatR.Businesses.Bank.Command;
using AsrTool.Infrastructure.MediatR.Businesses.Bank.Queries;
using AsrTool.Swagger.Account;
using AsrTool.Swagger.Bank;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AsrTool.Controllers
{
    public class BankController : BaseApiController
    {

        public BankController(IMediator mediator) : base(mediator)
        {

        }

        /// <summary>
        /// Get List of associate bank
        /// </summary>
        /// <returns>List of associate bank</returns>
        [HttpGet]
        public async Task<List<BankDto>> GetAll()
        {
            var result =  await Mediator.Send(new GetAllBankQuery());
            return result.ToList();
        }

        /// <summary>
        /// Create Bank
        /// </summary>
        /// <returns>Bank info, one key for hash, asymmetric encryption for associated bank</returns>
        /// <response code="400">Bad request: Failed to valid request body</response>
        /// <response code="401">Unauthorized: You are not Logged</response>
        /// <response code="406">Bussiness reason with message</response>
        [SwaggerResponseExample(200, typeof(BankExample))]
        [SwaggerRequestExample(typeof(CreateBankDto), typeof(CreateBankRequestExample))]
        [HttpPost]
        public async Task<CreateBankDto> CreateBank([FromBody] CreateBankDto createBankDto)
        {
            var result = await Mediator.Send(new CreateBankCommand
            {
                Request = createBankDto
            });
            return result;
        }

        /// <summary>
        /// Update Hash,asymmetric encryption key by bank name
        /// </summary>
        /// <returns>Bank name and Hash,asymmetric encryption key for associated bank</returns>
        /// <response code="400">Bad request: Failed to valid request body</response>
        /// <response code="401">Unauthorized: You are not Logged</response>
        /// <response code="406">Bussiness reason with message</response>
        [HttpPut]
        [SwaggerResponseExample(200, typeof(BankExample))]
        [SwaggerRequestExample(typeof(CreateBankDto), typeof(CreateBankRequestExample))]
        public async Task<CreateBankDto> EditBank([FromBody] CreateBankDto createBankDto)
        {
            var result = await Mediator.Send(new EditBankCommand
            {
                Request = createBankDto
            });
            return result;
        }


        /// <summary>
        /// Delete bank by id
        /// </summary>
        /// <returns>Status code</returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">Bad request: Failed to valid request body</response>
        /// <response code="401">Unauthorized: You are not Logged</response>
        /// <response code="406">Bussiness reason with message</response>
        [HttpDelete]
        public async Task DeleteBank([FromForm] int id)
        {
            await Mediator.Send(new DeleteBankCommand
            {
                id = id
            });
        }
    }
}
