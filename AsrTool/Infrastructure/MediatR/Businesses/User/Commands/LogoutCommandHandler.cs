using AsrTool.Infrastructure.Auth;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Commands
{
  public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
  {
    private readonly IUserManager _userManager;


    public LogoutCommandHandler(IUserManager userManager)
    {
      _userManager = userManager;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
      await _userManager.SignOut(request.HttpContext);
      return Unit.Value;
    }
  }
}