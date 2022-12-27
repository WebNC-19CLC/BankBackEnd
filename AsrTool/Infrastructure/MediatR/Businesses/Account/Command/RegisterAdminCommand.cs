using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class RegisterAdminCommand : IRequest
  {
    public RegisterRequestDto model;
    public HttpContext HttpContext { get; set; }
  }
}
