using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Commands
{
  public class LogoutCommand : IRequest
  {
    public HttpContext HttpContext { get; set; }
  }
}