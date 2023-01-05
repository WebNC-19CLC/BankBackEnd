using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using NetTopologySuite.IO;
using static AsrTool.Constants;

namespace AsrTool.Middlewares
{
    public class ThirdPartyMiddleware
    {
        private readonly RequestDelegate _next;
        public ThirdPartyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IAsrContext asrContext)
        {
            if (!httpContext.Request.Headers.TryGetValue(BankAuthenticateHeaderRequirement.SignatureHeader, out var signature))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsync("Token was not provided");
                return;
            }
            if (!httpContext.Request.Headers.TryGetValue(BankAuthenticateHeaderRequirement.TimeHeader, out var sendTime))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsync("Request send time was not provided");
                return;
            }
            if (!httpContext.Request.Headers.TryGetValue(BankAuthenticateHeaderRequirement.FromHeader, out var from))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsync("Bank name was not provided");
                return;
            }

            if (isTimeOut(sendTime))
            {
                httpContext.Response.StatusCode = 408;
                await httpContext.Response.WriteAsync("Request timeout");
                return;
            };

            var bank = await asrContext.Get<Bank>().SingleOrDefaultAsync(x => x.Name == from.ToString());

            if (bank == null)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                await httpContext.Response.WriteAsync("Bank name not found");
                return;
            }

            String uri = httpContext.Request.Path.ToString();
            String dataString = (String.Format("{0}{1}{2}", uri, sendTime, from));

            bool isAuthorized = Authorize(httpContext, signature, from, bank, dataString);

            if (!isAuthorized)
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("Unauthorized client");
                return;
            }

            await _next(httpContext);
        }

        private bool isTimeOut(StringValues sendTime)
        {
            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            UInt64 requestTimeStamp = Convert.ToUInt64(timeSpan.TotalMinutes);
            if (requestTimeStamp - Convert.ToUInt64(sendTime) > 1)
            {
                return true;
            }
            return false;
        }

        private bool Authorize(HttpContext httpContext, StringValues signature, StringValues from, Bank bank, string dataString)
        {
            bool isAuthorized = true;

            try
            {

                if (httpContext.Request.Method == HttpMethod.Get.Method)
                {
                    string hashedToken = EncryptionHelper.ComputeHash(bank.DecryptPublicKey, dataString);

                    if (!signature.ToString().Equals(hashedToken))
                    {
                        isAuthorized = false;
                    }
                }
                else if (httpContext.Request.Method == HttpMethod.Post.Method || httpContext.Request.Method == HttpMethod.Put.Method)
                {
                    RSAParameters pKey = EncryptionHelper.ConvertStringToRSAKey(bank.DecryptRsaPrivateKey);

                    string message = EncryptionHelper.RSADecryption(signature.ToString(), pKey);

                    if (!from.ToString().Equals(message.ToString()))
                    {
                        isAuthorized = false;
                    }
                    else
                    {
                        httpContext.Request.Headers.Add("BankSourceId", bank.Id.ToString());

                        Action<HttpResponse, Bank> SignSignature = ResponseHandler(bank.Name);
                        SignSignature(httpContext.Response, bank);
                    }
                }

                return isAuthorized;
            }
            catch (CryptographicException e)
            {
                isAuthorized = false;
            }
            return isAuthorized;
        }

        Action<HttpResponse, Bank> ResponseHandler(string bankName)
        {
            switch (bankName)
            {
                case Constants.AssociatedBank.RSA_BANK_NAME:
                    return RSAResponseHandler;
                case Constants.AssociatedBank.PGP_BANK_NAME:
                    return PGPResponseHandler;
                default:
                    return RSAResponseHandler;
            }

        }

        // RSA Security
        private static void RSAResponseHandler(HttpResponse response, Bank bank)
        {
            RSAParameters publicKey = EncryptionHelper.ConvertStringToRSAKey(bank.EncryptRsaPublicKey);

            string signature = EncryptionHelper.RSAEncryption(Constants.AssociatedBank.MY_BANK_NAME, publicKey);

            response.Headers.Add(Constants.BankAuthenticateHeaderRequirement.SignatureHeader, signature);

        }

        // PGP Security
        private static void PGPResponseHandler(HttpResponse response, Bank bank)
        {

        }

    }
}