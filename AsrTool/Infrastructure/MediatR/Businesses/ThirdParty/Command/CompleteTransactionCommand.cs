using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.ThirdParty.Command
{
    public class CompleteTransactionCommand : IRequest<TransactionDto>
    {
        public int Id { get; set; }
    }
}
