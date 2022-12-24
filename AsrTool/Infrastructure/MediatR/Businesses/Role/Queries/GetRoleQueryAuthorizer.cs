using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Common.Policy;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Queries
{
  public class GetRoleQueryAuthorizer : IAuthorizer<GetRoleQuery>
  {
    private readonly IAsrContext _context;

    public GetRoleQueryAuthorizer(IAsrContext context)
    {
      _context = context;
    }

    public async Task<AuthorizationResult> AuthorizeAsync(GetRoleQuery instance, CancellationToken cancellation = default)
    {
      var role = await _context.Get<Domain.Entities.Role>().SingleOrDefaultAsync(x => x.Id == instance.RoleId, cancellationToken: cancellation);
      return AuthorizationResult.Success();
    }
  }
}