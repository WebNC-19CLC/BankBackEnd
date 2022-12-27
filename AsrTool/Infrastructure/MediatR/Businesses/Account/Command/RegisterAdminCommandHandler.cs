using AsrTool.Infrastructure.Auth;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class RegisterAdminCommandHandler : IRequestHandler<RegisterAdminCommand>
  {
    private readonly IUserManager _userManager;

    public RegisterAdminCommandHandler(IUserManager userManager)
    {
      _userManager = userManager;
    }

    public async Task<Unit> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
    {
      await _userManager.RegisterAdmin(request.HttpContext, request.model);
      return Unit.Value;
    }
  }
}
