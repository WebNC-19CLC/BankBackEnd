using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Common.Policy;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Commands
{
  public class UpdateRoleCommandAuthorizer : IAuthorizer<UpdateRoleCommand>
  {
    private readonly IAsrContext _context;

    public UpdateRoleCommandAuthorizer(IAsrContext context)
    {
      _context = context;
    }

    public async Task<AuthorizationResult> AuthorizeAsync(UpdateRoleCommand instance, CancellationToken cancellation = default)
    {
      var roleInstance = _context.Get<Domain.Entities.Role>().SingleOrDefault(x => x.Id == instance.Role.Id);
      return AuthorizationResult.Success();
    }
  }
}