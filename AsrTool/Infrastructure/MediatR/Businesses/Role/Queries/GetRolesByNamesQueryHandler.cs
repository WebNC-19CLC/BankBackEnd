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
  public class GetRolesByNamesQueryHandler : IRequestHandler<GetRolesByNamesQuery, IEnumerable<RoleDto>>
  {
    private readonly IAsrContext _asrContext;
    private readonly IMapper _mapper;

    public GetRolesByNamesQueryHandler(IAsrContext asrContext, IMapper mapper)
    {
      _asrContext = asrContext;
      _mapper = mapper;
    }

    public async Task<IEnumerable<RoleDto>> Handle(GetRolesByNamesQuery request, CancellationToken cancellationToken)
    {
      var dbRoles = _asrContext.Get<Domain.Entities.Role>();

      if (request.Names?.Any() == true)
      {
        dbRoles = dbRoles.Where(r => request.Names.Contains(r.Name));
      }

      return await dbRoles
        .ProjectTo<RoleDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken: cancellationToken);
    }
  }
}
