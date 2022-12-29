using FluentValidation;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class ChangeMyPasswordCommandValidator : AbstractValidator<ChangeMyPasswordCommand>
  {
    public ChangeMyPasswordCommandValidator() {
      RuleFor(x => x.Request.Password).NotEmpty().NotNull();
      RuleFor(x => x.Request.NewPassword).NotEmpty().NotNull();
    }
  }
}
