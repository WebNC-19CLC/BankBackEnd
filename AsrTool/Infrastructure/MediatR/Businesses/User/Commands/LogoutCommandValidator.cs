using FluentValidation;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Commands
{
  public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
  {
    public LogoutCommandValidator()
    {
      RuleFor(x => x.HttpContext).NotNull();
    }
  }
}