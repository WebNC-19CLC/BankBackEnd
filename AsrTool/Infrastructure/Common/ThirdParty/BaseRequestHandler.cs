using AsrTool.Dtos;
using AsrTool.Infrastructure.Domain.Entities;

namespace AsrTool.Infrastructure.Common.ThirdParty
{
    public abstract class BaseRequestHandler : IThirdPartyRequestHandler
    {
        public readonly Bank bank;
        public BaseRequestHandler(Bank bank)
        {
            this.bank = bank;
        }

        public abstract HttpClient CommandCompleteTransaction(string toAccountNumber);
        public abstract HttpClient CommandMakeTransaction(MakeTransactionDto makeNotificationDto);
        public abstract HttpClient QueryInfo(string accountNumber);
    }
}
