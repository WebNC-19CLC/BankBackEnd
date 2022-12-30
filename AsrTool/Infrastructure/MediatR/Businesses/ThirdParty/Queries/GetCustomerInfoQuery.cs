using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.ThirdParty.Queries
{
    public class GetCustomerInfoQuery : IRequest<AccountDto>
    {
        public string AccountNumber { get; set; }
    }
}
