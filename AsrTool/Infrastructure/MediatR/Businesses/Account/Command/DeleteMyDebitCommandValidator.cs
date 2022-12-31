using FluentValidation;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class DeleteMyDebitCommandValidator : AbstractValidator<DeleteMyDebitCommand>
  {
    public DeleteMyDebitCommandValidator() {
      RuleFor(x => x.Id).GreaterThan(0);
    }
  }
}
