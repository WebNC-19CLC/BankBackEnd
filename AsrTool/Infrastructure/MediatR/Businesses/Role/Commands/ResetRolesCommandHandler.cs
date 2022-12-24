using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Context;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Commands
{
  public class ResetRolesCommandHandler : IRequestHandler<ResetRolesCommand, Unit>
  {
    private readonly IAsrContext _context;
    private readonly ICacheService _cacheService;

    public ResetRolesCommandHandler(IAsrContext context, ICacheService cacheService)
    {
      _context = context;
      _cacheService = cacheService;
    }

    public async Task<Unit> Handle(ResetRolesCommand command, CancellationToken cancellationToken)
    {
      var roleIds = command.Request.RoleIds;
      var dbRoles = _context.Get<Domain.Entities.Role>();

      if (roleIds?.Any() == true)
      {
        dbRoles = dbRoles.Where(r => roleIds.Contains(r.Id));
      }

      var updatedRoles = await dbRoles.ToListAsync(cancellationToken);
      foreach (var updatedRole in updatedRoles)
      {
        updatedRole.Rights = SharedRoles.GetRights(updatedRole.Name);
      }
      await _context.UpdateRangeAsync(updatedRoles);
      await _context.SaveChangesAsync();

      var dbUsernames = await _context.Get<Domain.Entities.Employee>().Select(r => r.Username).ToListAsync(cancellationToken);
      foreach (var username in dbUsernames)
      {
        _cacheService.RemoveAuthenticatedUserCache(username);
      }

      return Unit.Value;
    }
  }
}
