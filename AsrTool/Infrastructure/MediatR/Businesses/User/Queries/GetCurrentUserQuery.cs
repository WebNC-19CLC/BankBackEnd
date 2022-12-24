using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Queries
{
  public class GetCurrentUserQuery : IRequest<UserDto>
  {
    public HttpResponse HttpResponse { get; set; }
    public HttpContext HttpContext { get; set; }
  }
}