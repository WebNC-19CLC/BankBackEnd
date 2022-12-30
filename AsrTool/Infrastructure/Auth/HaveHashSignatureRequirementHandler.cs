using AsrTool.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography;
using System.Text;

namespace AsrTool.Infrastructure.Auth
{
    public class HaveHashSignatureRequirementHandler : AuthorizationHandler<HaveHashSignatureRequirement>
    {
        private readonly IAsrContext dbContext;
        public HaveHashSignatureRequirementHandler(IAsrContext context)
        {
            this.dbContext = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HaveHashSignatureRequirement requirement)
        {
            if(context.Resource is HttpContext httpContext)
            {
                if (!httpContext.Request.Headers.TryGetValue(requirement.SignatureHeader, out var signature))
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Token was not provided");
                    return;
                }
                if (!httpContext.Request.Headers.TryGetValue(requirement.TimeHeader, out var sendTime))
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Request send time was not provided");
                    return;
                }
                if (!httpContext.Request.Headers.TryGetValue(requirement.FromHeader, out var from))
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Bank name was not provided");
                    return;
                }

                TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
                UInt64 requestTimeStamp = Convert.ToUInt64(timeSpan.TotalMinutes);

                if (requestTimeStamp - Convert.ToUInt64(sendTime) > 1)
                {
                    httpContext.Response.StatusCode = 408;
                    await httpContext.Response.WriteAsync("Request timeout");
                    return;
                }

                string GetKeyFromDb = "hehe";
                String uri = httpContext.Request.Path.ToString();
                String authenticationDataString = (String.Format("{0}{1}{2}", uri, sendTime,from));


                string hashedToken = ComputeHash(GetKeyFromDb, authenticationDataString);

                if (!signature.ToString().Equals(hashedToken))
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Unauthorized client");
                    return;
                }
                context.Succeed(requirement); ;
            }
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
