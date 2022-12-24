using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AsrTool.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BaseApiController : ControllerBase
  {
    protected readonly IMediator Mediator;

    public BaseApiController(IMediator mediator)
    {
      Mediator = mediator;
    }
  }
}
