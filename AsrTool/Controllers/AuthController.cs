using AsrTool.Dtos;
using AsrTool.Infrastructure.MediatR.Businesses.User.Commands;
using AsrTool.Infrastructure.MediatR.Businesses.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsrTool.Controllers
{
  [Authorize]
  public class AuthController : BaseApiController
  {
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public async Task<bool> Login([FromBody] LoginRequestDto request)
    {
      await Mediator.Send(new LoginCommand()
      {
        Request = request,
        HttpContext = HttpContext
      });

      return true;
    }


    [HttpPost("register")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public async Task<bool> Register([FromBody] RegisterRequestDto request)
    {
      await Mediator.Send(new RegisterCommand()
      {
        model = request,
        HttpContext = HttpContext
      });

      return true;
    }

    [HttpGet("me")]
    [IgnoreAntiforgeryToken]
    public async Task<UserDto> GetCurrentUser()
    {
      return await Mediator.Send(new GetCurrentUserQuery()
      {
        HttpContext = HttpContext,
        HttpResponse = Response
      });
    }

    [HttpPost("logout")]
    [IgnoreAntiforgeryToken]
    public async Task Logout()
    {
      await Mediator.Send(new LogoutCommand() { HttpContext = HttpContext });
    }
  }
}
