using AsrTool.Dtos;
using AsrTool.Infrastructure.Domain.Enums;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.ReferenceData.Queries
{
  public class GetReferenceDataQuery : IRequest<IEnumerable<ReferenceDataResultDto>>
  {
    public IEnumerable<ReferenceDataType> Types { get; set; }
  }
}
