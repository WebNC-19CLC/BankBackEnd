using AsrTool.Dtos;
using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Common.Policy;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Commands
{
  public class CreateRoleCommandAuthorizer : IAuthorizer<CreateRoleCommand>
  {
    private readonly IMapper _mapper;

    public CreateRoleCommandAuthorizer(IMapper mapper)
    {
      _mapper = mapper;
    }

    public async Task<AuthorizationResult> AuthorizeAsync(CreateRoleCommand instance, CancellationToken cancellation = default)
    {
      return AuthorizationResult.Success();
    }
  }
}