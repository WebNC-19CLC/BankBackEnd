using AsrTool.Dtos;
using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Common.Policy;
using AsrTool.Infrastructure.Context;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Queries
{
  public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, RoleDto>
  {
    private readonly IAsrContext _asrContext;
    private readonly IMapper _mapper;

    public GetRoleQueryHandler(IAsrContext asrContext, IMapper mapper)
    {
      _asrContext = asrContext;
      _mapper = mapper;
    }

    public async Task<RoleDto> Handle(GetRoleQuery request, CancellationToken cancellationToken)
    {
      return await _asrContext.Get<Domain.Entities.Role>()
        .ProjectTo<RoleDto>(_mapper.ConfigurationProvider).SingleAsync(x => x.Id == request.RoleId, cancellationToken: cancellationToken);
    }
  }
}