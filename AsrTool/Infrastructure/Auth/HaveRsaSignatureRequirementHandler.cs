using AsrTool.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.Auth
{
    public class HaveHashPublicKeyRequirementHandler : AuthorizationHandler<HaveHashPublicKeyRequirementHandler>, IAuthorizationRequirement
    {
        private readonly IAsrContext _context;

        public HaveHashPublicKeyRequirementHandler(IAsrContext context)
        {
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HaveHashPublicKeyRequirementHandler requirement)
        {
            throw new NotImplementedException();
        }
    }
}
