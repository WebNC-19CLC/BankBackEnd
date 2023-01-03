using AsrTool.Dtos;

namespace AsrTool.Infrastructure.Common.ThirdParty
{
    public interface IThirdPartyRequestHandler
    {
        HttpClient QueryInfo(string accountNumber);
        HttpClient CommandMakeTransaction(MakeTransactionDto makeNotificationDto);
        HttpClient CommandCompleteTransaction(string transactionId);
    }
}
