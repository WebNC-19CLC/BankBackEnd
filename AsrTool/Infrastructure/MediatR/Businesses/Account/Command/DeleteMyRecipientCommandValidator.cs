using FluentValidation;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class DeleteMyRecipientCommandValidator : AbstractValidator<DeleteMyRecipientCommand>
  {
    public DeleteMyRecipientCommandValidator() {
      RuleFor(x => x.Id).GreaterThan(0);
    }
  }
}
