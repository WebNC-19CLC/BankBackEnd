using AsrTool.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Commands
{
  public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand>
  {
    private readonly IAsrContext _context;

    public DeleteRoleCommandHandler(IAsrContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
      var roleDb = await _context.Get<Domain.Entities.Role>().SingleAsync(x => x.Id == request.RoleId, cancellationToken: cancellationToken);
      await _context.RemoveAsync(roleDb);
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    }
  }
}