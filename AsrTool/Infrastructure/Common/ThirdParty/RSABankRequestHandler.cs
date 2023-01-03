using AsrTool.Dtos;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http.Headers;
using System.Security.Cryptography;

namespace AsrTool.Infrastructure.Common.ThirdParty
{
    public class RSABankRequestHandler : BaseRequestHandler
    {
        public const string HOST = "http://localhost:6001";
        public const string QUERY_INFO_PATH = "/api/thirdparty/customer/";
        public const string COMMAND_MAKE_TRANSACTION_PATH = "/api/thirdparty/transactions";
        public const string COMMAND_COMPLETE_TRANSACTION_PATH = "/api/thirdparty/transactions/";

        public RSABankRequestHandler(Bank bank) : base(bank)
        {
        }

        public override HttpClient CommandCompleteTransaction(string toAccountNumber)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(HOST);

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalMinutes).ToString();

            String dataString = (String.Format("{0}", "AsrTool"));

            RSAParameters privateKey = EncryptionHelper.ConvertStringToRSAKey(bank.EncryptRsaPublicKey);

            string signature = EncryptionHelper.RSAEncryption(dataString, privateKey);

            client.DefaultRequestHeaders.Add("TimeStamp", requestTimeStamp);
            client.DefaultRequestHeaders.Add("BankSource", bank.Name);
            client.DefaultRequestHeaders.Add("XApiKey", signature);

            return client;
        }

        public override HttpClient CommandMakeTransaction(MakeTransactionDto makeNotificationDto)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(HOST);

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalMinutes).ToString();

            String dataString = (String.Format("{0}", "AsrTool"));

            RSAParameters privateKey = EncryptionHelper.ConvertStringToRSAKey(bank.EncryptRsaPublicKey);

            string signature = EncryptionHelper.RSAEncryption(dataString,privateKey);

            client.DefaultRequestHeaders.Add("TimeStamp", requestTimeStamp);
            client.DefaultRequestHeaders.Add("BankSource", bank.Name);
            client.DefaultRequestHeaders.Add("XApiKey", signature);

            return client;
        }

        public override HttpClient QueryInfo(string accountNumber)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(HOST);
            
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalMinutes).ToString();

            String dataString = (String.Format("{0}{1}{2}", QUERY_INFO_PATH, requestTimeStamp, bank.Name));
            string HashKey = bank.EncryptRsaPublicKey;

            string hashedToken = EncryptionHelper.ComputeHash(HashKey, dataString);

            client.DefaultRequestHeaders.Add("TimeStamp", requestTimeStamp);
            client.DefaultRequestHeaders.Add("BankSource", bank.Name);
            client.DefaultRequestHeaders.Add("XApiKey", hashedToken);

            return client;
        }
    }
}
