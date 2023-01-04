using AsrTool.Dtos;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AsrTool.Infrastructure.Helpers;
using System.Security.Cryptography;
using static AsrTool.Constants;

namespace AsrTool.Infrastructure.Common.ThirdParty
{
    public abstract class BaseRequestHandler : IThirdPartyRequestHandler
    {
        public string HOST;
        public string QUERY_INFO_PATH;
        public string COMMAND_MAKE_TRANSACTION_PATH;
        public string COMMAND_COMPLETE_TRANSACTION_PATH;

        public readonly Bank associatedBank;
        public BaseRequestHandler(Bank bank)
        {
            this.associatedBank = bank;
        }

        protected void ValidSignature(HttpResponseMessage httpResponse)
        {
            var signature = httpResponse.Headers.GetValues(BankAuthenticateHeaderRequirement.SignatureHeader);

            if(signature == null)
            {
                throw new BusinessException("Failed to make transaction");
            }
            
            RSAParameters privateKey = EncryptionHelper.ConvertStringToRSAKey(signature.ToString());

            string decryptMessage = EncryptionHelper.RSADecryption(signature.ToString(), privateKey);

            if (!decryptMessage.Equals(associatedBank.Name)) ;
            {
                throw new BusinessException("Failed to make transaction");
            }
        }

        public abstract Task CommandCompleteTransaction(string transactionId);
        public abstract Task<TransactionDto> CommandMakeTransaction(MakeTransactionDto makeNotificationDto);
        public abstract Task<ShortAccountDto> QueryInfo(string accountNumber);
    }
}
