using System.Net;
using System.Security.Cryptography;
using System.Text;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace AsrTool.Middlewares
{
    public class ValidSignatureThirdPartyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string SignatureHeader = "XApiKey";
        private const string TimeHeader = "TimeStamp";
        private const string FromHeader = "BankSource";

        public ValidSignatureThirdPartyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IAsrContext asrContext)
        {
            string a = httpContext.Request.Method;
            //skip
            await _next(httpContext);
            return;

            if (!httpContext.Request.Headers.TryGetValue(SignatureHeader, out var signature))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsync("Token was not provided");
                return;
            }
            if (!httpContext.Request.Headers.TryGetValue(TimeHeader, out var sendTime))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsync("Request send time was not provided");
                return;
            }
            if (!httpContext.Request.Headers.TryGetValue(FromHeader, out var from))
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

            if(bank == null)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                await httpContext.Response.WriteAsync("Bank name not found");
                return;
            }

            String uri = httpContext.Request.Path.ToString();
            String authenticationDataString = (String.Format("{0}{1}{2}", uri, sendTime, from));

            string hashedToken = ComputeHash(bank.DecryptPublicKey, authenticationDataString);

            if (!signature.ToString().Equals(hashedToken))
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

        private string ComputeHash(String secretKey, String authenticationDataString)
        {
            HMACSHA512 hmac = new HMACSHA512(Convert.FromBase64String(secretKey));

            Byte[] authenticationData = UTF8Encoding.GetEncoding("utf-8").GetBytes(authenticationDataString);

            var hashedToken = hmac.ComputeHash(authenticationData);
            return Convert.ToBase64String(hashedToken);
        }
    }
}