using AsrTool.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AsrTool.Infrastructure.Filters
{
    public class SecureFilterAttribute : ActionFilterAttribute
    {
        private readonly AsrContext _context;
        private IBankSecurityService? securityService;

        public SecureFilterAttribute(AsrContext context)
        {
            _context = context;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return base.OnActionExecutionAsync(context, next);
        }


    }
}
