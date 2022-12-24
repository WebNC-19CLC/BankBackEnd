using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Commands
{
  public class AssignUsersToRoleCommandHandler : IRequestHandler<AssignUsersToRoleCommand, Unit>
  {
    private readonly IAsrContext _context;
    private readonly ICacheService _cacheService;

    public AssignUsersToRoleCommandHandler(IAsrContext context, ICacheService cacheService)
    {
      _context = context;
      _cacheService = cacheService;
    }

    public async Task<Unit> Handle(AssignUsersToRoleCommand command, CancellationToken cancellationToken)
    {
      var userIds = command.Request.UserIds;
      var removeCurrentRole = command.Request.RemoveCurrentRole == true;
      if (!userIds.Any())
      {
        return Unit.Value;
      }

      int? roleToAssign = null;
      if (!removeCurrentRole)
      {
        var requestRoleId = command.Request.RoleId;
        var dbRole = await _context.Get<Domain.Entities.Role>().SingleOrDefaultAsync(r => r.Id == requestRoleId, cancellationToken);
        if (dbRole == null)
        {
          throw new NotFoundException<Domain.Entities.Role>(requestRoleId.Value);
        }
        roleToAssign = dbRole.Id;
      }

      var dbUsers = await _context.Get<Domain.Entities.Employee>()
        .Where(x => userIds.Contains(x.Id)).ToListAsync(cancellationToken);
      if (!dbUsers.Any())
      {
        return Unit.Value;
      }

      foreach (var updatedUser in dbUsers)
      {
        updatedUser.RoleId = roleToAssign;
      }
      await _context.UpdateRangeAsync(dbUsers);
      await _context.SaveChangesAsync(cancellationToken);

      foreach (var updatedUser in dbUsers)
      {
        _cacheService.RemoveAuthenticatedUserCache(updatedUser.Username);
      }

      return Unit.Value;
    }
  }
}
