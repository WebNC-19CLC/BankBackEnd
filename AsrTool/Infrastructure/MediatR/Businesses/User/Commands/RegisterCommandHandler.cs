using AsrTool.Infrastructure.Auth;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Commands
{
  public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
  {
    private readonly IUserManager _userManager;

    public RegisterCommandHandler(IUserManager userManager)
    {
      _userManager = userManager;
    }

    public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
      await _userManager.Register(request.HttpContext, request.model);
      return Unit.Value;
    }
  }
}
