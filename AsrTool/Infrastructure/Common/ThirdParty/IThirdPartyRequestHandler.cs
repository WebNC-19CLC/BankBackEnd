using AsrTool.Dtos;

namespace AsrTool.Infrastructure.Common.ThirdParty
{
    public interface IThirdPartyRequestHandler
    {
        Task<AccountDto> QueryInfo(string accountNumber);
        Task<TransactionDto> CommandMakeTransaction(MakeTransactionDto makeNotificationDto);
        Task CommandCompleteTransaction(string transactionId);
    }
}
