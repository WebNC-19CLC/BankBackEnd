using FluentValidation;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Commands
{
  public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
  {
    public RegisterCommandValidator() {
      RuleFor(x => x.model.Username).NotNull().NotEmpty();
      RuleFor(x => x.model.FirstName).NotNull().NotEmpty();
      RuleFor(x => x.model.LastName).NotNull().NotEmpty();
      RuleFor(x => x.model.Email).NotNull().NotEmpty();
      RuleFor(x => x.model.Password).NotNull().NotEmpty();
      RuleFor(x => x.HttpContext).NotNull();
    }
  }
}
