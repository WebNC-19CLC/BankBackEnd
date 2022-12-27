using AsrTool.Dtos;
using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.MediatR.Businesses.ReferenceData.Queries;
using AsrTool.Infrastructure.MediatR.Businesses.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsrTool.Controllers
{
  [Authorize]
  public class ReferenceDataController : BaseApiController
  {

    public ReferenceDataController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost()]
    public async Task<IEnumerable<ReferenceDataResultDto>> GetReferenceData([FromBody] IEnumerable<ReferenceDataType> types)
    {
      return await Mediator.Send(new GetReferenceDataQuery { Types = types });
    }

    [HttpPost("searchUsersByTerm")]
    public async Task<IEnumerable<UserRefDto>> SearchUsersByTerm([FromBody] SearchUsersByTermFilterDto request)
    {
      return await Mediator.Send(new SearchUsersByTermQuery()
      {
        Request = request
      });
    }
  }
}
