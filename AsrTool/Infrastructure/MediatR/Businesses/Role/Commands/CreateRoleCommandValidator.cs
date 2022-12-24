using FluentValidation;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Commands
{
  public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
  {
    public CreateRoleCommandValidator()
    {
      RuleFor(x => x.Role).NotNull();
      RuleFor(x => x.Role.Name).NotEmpty().When(x => x.Role != null);
      RuleFor(x => x.Role.Id).NotNull().Equal(0).When(x => x.Role != null); ;
      RuleFor(x => x.Role.Rights).NotEmpty().When(x => x.Role != null); ;
    }
  }
}