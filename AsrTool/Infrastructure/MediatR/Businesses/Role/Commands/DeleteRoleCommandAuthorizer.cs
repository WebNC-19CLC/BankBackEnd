using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Common.Policy;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Commands
{
  public class DeleteRoleCommandAuthorizer : IAuthorizer<DeleteRoleCommand>
  {
    private readonly IAsrContext _context;

    public DeleteRoleCommandAuthorizer(IAsrContext context)
    {
      _context = context;
    }

    public async Task<AuthorizationResult> AuthorizeAsync(DeleteRoleCommand instance, CancellationToken cancellation = default)
    {
      var roleInstance = _context.Get<Domain.Entities.Role>().SingleOrDefault(x => x.Id == instance.RoleId);
      return AuthorizationResult.Success();
    }
  }
}