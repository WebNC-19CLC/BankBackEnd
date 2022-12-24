using FluentValidation;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Commands
{
  public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
  {
    public DeleteRoleCommandValidator()
    {
      RuleFor(x => x.RoleId).NotNull().GreaterThan(0);
    }
  }
}