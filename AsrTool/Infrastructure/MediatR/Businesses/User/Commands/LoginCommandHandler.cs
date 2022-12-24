using AsrTool.Infrastructure.Auth;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Commands
{
  public class LoginCommandHandler : IRequestHandler<LoginCommand>
  {
    private readonly IUserManager _userManager;

    public LoginCommandHandler(IUserManager userManager)
    {
      _userManager = userManager;
    }

    public async Task<Unit> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
      var normalizedUserName = request.Request.Username?.Split(new string[] { "\\", "/" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
      await _userManager.SignIn(request.HttpContext, normalizedUserName, request.Request.Password);
      return Unit.Value;
    }
  }
}