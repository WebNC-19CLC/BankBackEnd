using AsrTool.Dtos;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AsrTool.Infrastructure.Helpers;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace AsrTool.Infrastructure.Common.ThirdParty
{
    public class RSABankRequestHandler : BaseRequestHandler
    {
        public RSABankRequestHandler(Bank bank) : base(bank)
        {
            HOST = "http://localhost:6001";
            QUERY_INFO_PATH = "/api/thirdparty/customer/";
            COMMAND_MAKE_TRANSACTION_PATH = "/api/thirdparty/transactions";
            COMMAND_COMPLETE_TRANSACTION_PATH = "/api/thirdparty/transactions/";
        }

        public override async Task CommandCompleteTransaction(string transactionId)
        {
            var client = CommonHttpClientHeaderRequirement();

            String dataString = (String.Format("{0}", Constants.AssociatedBank.MY_BANK_NAME));

            RSAParameters privateKey = EncryptionHelper.ConvertStringToRSAKey(associatedBank.EncryptRsaPublicKey);

            string signature = EncryptionHelper.RSAEncryption(dataString, privateKey);

            client.DefaultRequestHeaders.Add("XApiKey", signature);

            var response =  await client.PutAsync(COMMAND_COMPLETE_TRANSACTION_PATH + transactionId, null);

            if (!response.IsSuccessStatusCode)
                throw new BusinessException("Failed to make transaction");

            return;
        }

        public override async Task<TransactionDto> CommandMakeTransaction(MakeTransactionDto makeNotificationDto)
        {
            var client = CommonHttpClientHeaderRequirement();

            String dataString = (String.Format("{0}", Constants.AssociatedBank.MY_BANK_NAME));

            RSAParameters privateKey = EncryptionHelper.ConvertStringToRSAKey(associatedBank.EncryptRsaPublicKey);

            string signature = EncryptionHelper.RSAEncryption(dataString,privateKey);

            client.DefaultRequestHeaders.Add("XApiKey", signature);

            var bodyJson = JsonConvert.SerializeObject(makeNotificationDto);
            var payload = new StringContent(bodyJson, Encoding.UTF8, "application/json");

            var response =  await client.PostAsync(COMMAND_MAKE_TRANSACTION_PATH, payload);

            if (!response.IsSuccessStatusCode)
                throw new BusinessException("Failed to make transaction");

            // Valid signature 
            base.ValidSignature(response);

            var responseObject = response.Content.ReadAsStringAsync().Result;

            TransactionDto transaction = JsonConvert.DeserializeObject<TransactionDto>(responseObject);

            return transaction;
        }

        public override async Task<AccountDto> QueryInfo(string accountNumber)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(HOST);

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalMinutes).ToString();

            client.DefaultRequestHeaders.Add("TimeStamp", requestTimeStamp);
            client.DefaultRequestHeaders.Add("BankSource", Constants.AssociatedBank.MY_BANK_NAME);

            String dataString = (String.Format("{0}{1}{2}", QUERY_INFO_PATH, requestTimeStamp, Constants.AssociatedBank.MY_BANK_NAME));

            string HashKey = associatedBank.EncryptRsaPublicKey;

            string hashedToken = EncryptionHelper.ComputeHash(HashKey, dataString);

            client.DefaultRequestHeaders.Add("XApiKey", hashedToken);

            var response = await client.GetAsync(QUERY_INFO_PATH + accountNumber);


            if (!response.IsSuccessStatusCode)
                throw new BusinessException("Failed to fetch bank account info");

            var responseObject = response.Content.ReadAsStringAsync().Result;

            AccountDto bankAccountDto = JsonConvert.DeserializeObject<AccountDto>(responseObject);

            return bankAccountDto;
        }

        private HttpClient CommonHttpClientHeaderRequirement()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(HOST);

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalMinutes).ToString();

            client.DefaultRequestHeaders.Add("TimeStamp", requestTimeStamp);
            client.DefaultRequestHeaders.Add("BankSource", Constants.AssociatedBank.MY_BANK_NAME);

            return client;
        }
    }
}
