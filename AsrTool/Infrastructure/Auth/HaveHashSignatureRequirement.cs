using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.Auth
{
    public class HaveHashSignatureRequirement : IAuthorizationRequirement
    {
        public string TimeHeader;
        public string SignatureHeader;
        public string FromHeader;

        public HaveHashSignatureRequirement(string signatureHeader, string timeHeader, string fromHeader)
        {
            SignatureHeader = signatureHeader;
            TimeHeader = timeHeader;
            FromHeader = fromHeader;
        }
    }
}
