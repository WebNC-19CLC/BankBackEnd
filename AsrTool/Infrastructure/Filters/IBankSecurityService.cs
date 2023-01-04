using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AsrTool.Infrastructure.Filters
{
    public interface IBankSecurityService
    {
        void Secure(Bank bank, ActionExecutingContext context);
    }
}
