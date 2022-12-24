using System.Net;
using AsrTool.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Middlewares
{
  public class GrantPermissionsMiddleware
  {
    private readonly RequestDelegate _next;

    public GrantPermissionsMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      if (context.User.Identity?.IsAuthenticated == true)
      {
        var userManager = context.RequestServices.GetService<IUserManager>();
        await userManager.GrantPermissions(context);
        await _next(context);
        return;
      }

      if (HasAnonymousAttribute(context))
      {
        await _next(context);
        return;
      }

      context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    }

    private static bool HasAnonymousAttribute(HttpContext context)
    {
      var endpoint = context.GetEndpoint();
      if (endpoint == null)
      {
        return true;
      }
      var retVal = (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null);
      return retVal;
    }
  }
}