using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Commands
{
  public class RegisterCommand : IRequest
  {
    public RegisterRequestDto model;
    public HttpContext HttpContext { get; set; }
  }
}
