using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Helpers;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Commands
{
  public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand>
  {
    private readonly IAsrContext _context;
    private readonly IMapper _mapper;

    public UpdateRoleCommandHandler(IAsrContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
      var role = _mapper.Map<RoleDto, Domain.Entities.Role>(request.Role);
      var roleDb = await _context.Get<Domain.Entities.Role>().SingleAsync(x => x.Id == request.Role.Id, cancellationToken: cancellationToken);
      EntityHelper.AdaptAuditableData(roleDb, role);
      await _context.UpdateAsync(role);
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    }
  }
}