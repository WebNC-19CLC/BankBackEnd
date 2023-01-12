using AsrTool.Dtos;
using AsrTool.Infrastructure.MediatR.Businesses.Account.Command;
using AsrTool.Infrastructure.MediatR.Businesses.User.Commands;
using AsrTool.Infrastructure.MediatR.Businesses.User.Queries;
using AsrTool.Swagger.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace AsrTool.Controllers
{
  public class AuthController : BaseApiController
  {
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <returns>Status code</returns>
    /// <response code="200">Success</response> 
    /// <response code="400">Bad request: Failed to valid request body</response>
    /// <response code="406">Bussiness reason with message</response>
    [SwaggerRequestExample(typeof(LoginRequestDto), typeof(AuthExample))]
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

    /// <summary>
    /// Register
    /// </summary>
    /// <returns>Status code</returns>
    /// <response code="200">Success</response> 
    /// <response code="400">Bad request: Failed to valid request body</response>
    /// <response code="406">Bussiness reason with message</response>
    [SwaggerRequestExample(typeof(RegisterRequestDto), typeof(RegisterRequestExample))]
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


    /// <summary>
    /// Get current user detail
    /// </summary>
    /// <returns>User detail</returns>
    /// <response code="200">Success</response> 
    /// <response code="400">Bad request: Failed to valid request body</response>
    /// <response code="406">Bussiness reason with message</response>
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

    /// <summary>
    /// Logout
    /// </summary>
    /// <returns>Status code</returns>
    /// <response code="200">Success</response> 
    /// <response code="400">Bad request: Failed to valid request body</response>
    /// <response code="406">Bussiness reason with message</response>
    [HttpPost("logout")]
    [IgnoreAntiforgeryToken]
    public async Task Logout()
    {
      await Mediator.Send(new LogoutCommand() { HttpContext = HttpContext });
    }

    /// <summary>
    ///  Generate OTP command
    /// </summary>
    /// <returns>Status code</returns>
    /// <response code="200">Success</response> 
    /// <response code="400">Bad request: Failed to valid request body</response>
    /// <response code="406">Bussiness reason with message</response>
    [SwaggerRequestExample(typeof(ForgotPasswordGenOTPDto), typeof(ForgotPasswordGenOTPDtoExample))]
    [HttpPost("generate-otp-forgot-password")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public async Task GenerateOTPForgotPassword(ForgotPasswordGenOTPDto dto)
    {
      await Mediator.Send(new GenerateOTPCommand() {Username = dto.Username });
    }

    /// <summary>
    /// Validate OTP
    /// </summary>
    /// <returns>Status code</returns>
    /// <response code="200">Success</response> 
    /// <response code="400">Bad request: Failed to valid request body</response>
    /// <response code="406">Bussiness reason with message</response>
    [SwaggerRequestExample(typeof(ValidateOTPForgotPasswordDto), typeof(ValidateOTPForgotPasswordDtoExample))]
    [HttpPost("validate-otp-forgot-password")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public async Task ValidateOTPForgotpassword(ValidateOTPForgotPasswordDto dto)
    {
      await Mediator.Send(new ValidateForgotPasswordCommand() {Request = dto });
    }

    /// <summary>
    /// Forgot password change password command
    /// </summary>
    /// <returns>Status code</returns>
    /// <response code="200">Success</response> 
    /// <response code="400">Bad request: Failed to valid request body</response>
    /// <response code="406">Bussiness reason with message</response>
    [SwaggerRequestExample(typeof(ForgotPasswordDto), typeof(ForgotPasswordDtoExample))]
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public async Task ForgotPassword(ForgotPasswordDto dto)
    {
      await Mediator.Send(new ForgotPasswordCommand() { Request = dto });
    }
  }
}
