using FluentValidation;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Commands
{
  public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
  {
    public UpdateRoleCommandValidator()
    {
      RuleFor(x => x.Role).NotNull();
      RuleFor(x => x.Role.Name).NotEmpty().When(x => x.Role != null);
      RuleFor(x => x.Role.Id).NotNull().GreaterThan(0).When(x => x.Role != null);
      RuleFor(x => x.Role.Rights).NotEmpty().When(x => x.Role != null);
      RuleFor(x => x.Role.RowVersion).NotEmpty().When(x => x.Role != null);
    }
  }
}