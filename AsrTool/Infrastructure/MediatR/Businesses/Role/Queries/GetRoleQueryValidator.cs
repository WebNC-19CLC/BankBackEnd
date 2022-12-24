using FluentValidation;

namespace AsrTool.Infrastructure.MediatR.Businesses.Role.Queries
{
  public class GetRoleQueryValidator : AbstractValidator<GetRoleQuery>
  {
    public GetRoleQueryValidator()
    {
      RuleFor(x => x.RoleId).NotNull().GreaterThan(0);
    }
  }
}