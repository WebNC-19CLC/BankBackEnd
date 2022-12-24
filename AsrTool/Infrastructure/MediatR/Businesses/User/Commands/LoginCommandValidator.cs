using FluentValidation;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Commands
{
  public class LoginCommandValidator : AbstractValidator<LoginCommand>
  {
    public LoginCommandValidator()
    {
      RuleFor(x => x.Request).NotNull();
      RuleFor(x => x.HttpContext).NotNull();
      RuleFor(x => x.Request.Username).NotEmpty().When(x => x.Request != null);
      RuleFor(x => x.Request.Password).NotEmpty().When(x => x.Request != null);
    }
  }
}