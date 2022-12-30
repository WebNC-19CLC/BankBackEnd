using AsrTool.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.Auth
{
    public class HaveRsaSignatureRequirementHandler : AuthorizationHandler<HaveRsaSignatureRequirement>, IAuthorizationRequirement
    {
        private readonly IAsrContext _context;

        public HaveRsaSignatureRequirementHandler(IAsrContext context)
        {
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HaveRsaSignatureRequirement requirement)
        {
            throw new NotImplementedException();
        }
    }
}
