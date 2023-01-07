using AsrTool.Dtos;
using AsrTool.Infrastructure.MediatR.Businesses.Bank.Command;
using AsrTool.Infrastructure.MediatR.Businesses.Bank.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AsrTool.Controllers
{
    public class BankController : BaseApiController
    {
        public BankController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<List<BankDto>> GetAll()
        {
            var result =  await Mediator.Send(new GetAllBankQuery());
            return result.ToList();
        }

        [HttpPost]
        public async Task<CreateBankDto> CreateBank([FromBody] CreateBankDto createBankDto)
        {
            var result = await Mediator.Send(new CreateBankCommand
            {
                Request = createBankDto
            });
            return result;
        }
    }
}
