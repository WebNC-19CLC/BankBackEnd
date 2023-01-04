using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.ThirdParty.Queries
{
    public class GetCustomerInfoQuery : IRequest<ShortAccountDto>
    {
        public string AccountNumber { get; set; }
    }
}
