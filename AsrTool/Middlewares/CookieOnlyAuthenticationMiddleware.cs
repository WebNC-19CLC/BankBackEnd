using System.Net;
using System.Security.Principal;
using AsrTool.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Middlewares
{
  public class CookieOnlyAuthenticationMiddleware
  {
    private readonly RequestDelegate _next;

    public CookieOnlyAuthenticationMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      if (context.User.Identity?.IsAuthenticated == true && context.User.Identity is WindowsIdentity)
      {
        var userManager = context.RequestServices.GetService<IUserManager>();
        await userManager.SignIn(context, string.Empty, string.Empty);
        await _next(context);
        return;
      }

      if (context.User.Identity?.IsAuthenticated == true || HasAnonymousAttribute(context))
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