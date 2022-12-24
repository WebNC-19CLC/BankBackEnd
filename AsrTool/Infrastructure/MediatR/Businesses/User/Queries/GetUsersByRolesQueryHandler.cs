using AsrTool.Dtos;
using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Common.Grid;
using AsrTool.Infrastructure.Common.Policy;
using AsrTool.Infrastructure.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Queries
{
  public class GetUsersByRolesQueryHandler : GridQueryHandler<GetUsersByRolesQuery, Domain.Entities.Employee, UserByRoleDto>
  {
    private readonly IAsrContext _asrContext;

    public GetUsersByRolesQueryHandler(IAsrContext asrContext, IMapper mapper) : base(mapper)
    {
      _asrContext = asrContext;
    }

    public override async Task<DataSourceResultDto<UserByRoleDto>> Handle(GetUsersByRolesQuery request, CancellationToken cancellationToken)
    {
      var allowedRoleIds = await _asrContext.Get<Domain.Entities.Role>()
        .Select(r => r.Id)
        .ToListAsync(cancellationToken);

      var requestedRoleIds = request.RoleIds ?? Array.Empty<int>();
      request.RoleIds = requestedRoleIds.Any() ? requestedRoleIds.Intersect(allowedRoleIds).ToList() : allowedRoleIds;

      request.Query = _asrContext.Get<Domain.Entities.Employee>()
        .Where(x => x.RoleId.HasValue && request.RoleIds.Contains(x.RoleId.Value));

      return await base.Handle(request, cancellationToken);
    }
  }
}
