using AsrTool.Dtos;

namespace AsrTool.Infrastructure.Common.ThirdParty
{
    public interface IThirdPartyRequestHandler
    {
        Task<ShortAccountDto> QueryInfo(string accountNumber);
        Task<TransactionDto> CommandMakeTransaction(MakeTransactionDto makeNotificationDto);
        Task CommandCompleteTransaction(string transactionId);
    }
}
