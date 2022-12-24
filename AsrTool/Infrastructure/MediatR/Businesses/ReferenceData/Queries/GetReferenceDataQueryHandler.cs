using AsrTool.Dtos;
using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Context;
using AutoMapper;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.ReferenceData.Queries
{
  public class GetReferenceDataQueryHandler : IRequestHandler<GetReferenceDataQuery, IEnumerable<ReferenceDataResultDto>>
  {
    private readonly IAsrContext _asrContext;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public GetReferenceDataQueryHandler(IAsrContext asrContext, IMapper mapper, ICacheService cacheService)
    {
      _asrContext = asrContext;
      _mapper = mapper;
      _cacheService = cacheService;
    }

    public Task<IEnumerable<ReferenceDataResultDto>> Handle(GetReferenceDataQuery request, CancellationToken cancellationToken)
    {
      var result = new List<ReferenceDataResultDto>();

      foreach (var type in request.Types)
      {
        switch (type)
        {
          //TODO: implement get reference data for corresponding type
          default:
            break;
        }
      }

      return Task.FromResult(result.AsEnumerable());
    }

    //private async Task<IEnumerable<ReferenceDataDto>> GetData<T>(CancellationToken cancellationToken) where T : class, IIdentity
    //{
    //  return await _asrContext.Get<T>().ProjectTo<ReferenceDataDto>(_mapper.ConfigurationProvider).OrderBy(x => x.Label).ToListAsync(cancellationToken);
    //}

    //private async Task<ReferenceDataResultDto> GetWorkplaceData(ReferenceDataType type, CancellationToken cancellationToken)
    //{
    //  return (await _cacheService.GetWorkplaceReferenceCacheData(cancellationToken)).FirstOrDefault(x => x.Type == type);
    //}
  }
}
