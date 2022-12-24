using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.Extensions;
using Hangfire.Dashboard;

namespace AsrTool.Infrastructure.Jobs
{
  public class HangfireAuthorization : IDashboardAuthorizationFilter
  {
    public bool Authorize(DashboardContext context)
    {
      var httpContext = context.GetHttpContext();
      if (httpContext?.User.Identity == null || httpContext.User.Identity.IsAuthenticated == false)
      {
        return false;
      }

      var userName = httpContext.User.Identity.Name!;
      var caching = httpContext.RequestServices.GetService<ICacheService>() ?? throw new InvalidDataException();
      var user = caching.GetEmployeeCachingItem(userName).RunAwait();
      return user?.Rights?.Any(x => x == Right.JobRunner) == true;
    }
  }
}