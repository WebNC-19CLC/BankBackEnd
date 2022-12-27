using AsrTool.Dtos;

namespace AsrTool.Infrastructure.Auth
{
  public interface IUserManager
  {
    Task SignIn(HttpContext httpContext, string username, string password, bool isPersistent = false);

    Task Register(HttpContext httpContext, RegisterRequestDto model);

    Task RegisterAdmin(HttpContext httpContext, RegisterRequestDto model);

    Task SignOut(HttpContext httpContext);

    Task GrantPermissions(HttpContext httpContext);
  }
}