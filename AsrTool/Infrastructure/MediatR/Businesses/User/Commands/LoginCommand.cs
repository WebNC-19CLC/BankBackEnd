using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Commands
{
  public class LoginCommand : IRequest
  {
    public LoginRequestDto Request { get; set; }
    public HttpContext HttpContext { get; set; }
  }
}