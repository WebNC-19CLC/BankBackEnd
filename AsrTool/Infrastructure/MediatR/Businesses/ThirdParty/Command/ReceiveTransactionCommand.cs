using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.ThirdParty.Command
{
    public class ReceiveTransactionCommand : IRequest<TransactionDto>
    {
        public TransactionDto transaction { get; set; }
    }
}
