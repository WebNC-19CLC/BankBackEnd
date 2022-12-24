using FluentValidation;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Queries
{
  public class GetCurrentUserQueryValidator : AbstractValidator<GetCurrentUserQuery>
  {
    public GetCurrentUserQueryValidator()
    {
      RuleFor(x => x.HttpResponse).NotNull();
      RuleFor(x => x.HttpContext).NotNull();
    }
  }
}