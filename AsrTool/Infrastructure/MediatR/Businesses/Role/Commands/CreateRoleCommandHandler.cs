using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using AutoMapper;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Commands
{
  public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand>
  {
    private readonly IAsrContext _context;
    private readonly IMapper _mapper;

    public CreateRoleCommandHandler(IAsrContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<Unit> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
      var role = _mapper.Map<RoleDto, Domain.Entities.Role>(request.Role);

      await _context.AddAsync(role);
      await _context.SaveChangesAsync(cancellationToken);

      return Unit.Value;
    }
  }
}