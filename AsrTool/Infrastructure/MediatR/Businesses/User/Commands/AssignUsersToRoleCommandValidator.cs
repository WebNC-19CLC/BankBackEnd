using FluentValidation;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Commands
{
  public class AssignUsersToRoleCommandValidator : AbstractValidator<AssignUsersToRoleCommand>
  {
    public AssignUsersToRoleCommandValidator()
    {
      RuleFor(x => x.Request).NotNull();
      RuleFor(x => x.Request.UserIds).NotEmpty();
      RuleFor(x => x.Request.RoleId).NotNull().When(x => !x.Request.RemoveCurrentRole.HasValue || !x.Request.RemoveCurrentRole.Value);
    }
  }
}
