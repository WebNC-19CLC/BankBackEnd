using System.Linq.Expressions;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.Common.Policy
{
  public class EmployeeAuthorizationHandler : AuthorizationHandlerBase<Employee>
  {
    private readonly IUserResolver _userResolver;

    public EmployeeAuthorizationHandler(IUserResolver userResolver)
    {
      _userResolver = userResolver;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement, Employee resource)
    {
      if (resource == null)
      {
        return Task.CompletedTask;
      }

      var validationFunc = GetExpressionAuthorizationRequirement(_userResolver.CurrentUser, requirement).Compile();
      if (validationFunc(resource))
      {
        context.Succeed(requirement);
        return Task.CompletedTask;
      }

      return Task.CompletedTask;
    }

    public override Expression<Func<Employee, bool>> GetExpressionAuthorizationRequirement(IUser user, AuthorizationRequirement requirement)
    {
      if (AuthorizationRequirement.Create.Equals(requirement))
      {
        if (user.IsContainsAllRights(Right.WriteEmployeeAll))
        {
          return x => true;
        }

        if (user.IsContainsAllRights(Right.WriteEmployeeInBL))
        {
          return x => x.OrganizationUnit == user.OrganizationUnit;
        }
      }

      if (AuthorizationRequirement.Read.Equals(requirement))
      {
        if (user.IsContainsAllRights(Right.ReadEmployeeAll))
        {
          return x => true;
        }

        if (user.IsContainsAllRights(Right.ReadEmployeeInBL))
        {
          return x => x.OrganizationUnit == user.OrganizationUnit;
        }
      }

      if (AuthorizationRequirement.Update.Equals(requirement))
      {
        if (user.IsContainsAllRights(Right.WriteEmployeeAll))
        {
          return x => true;
        }

        if (user.IsContainsAllRights(Right.WriteEmployeeInBL))
        {
          return x => x.OrganizationUnit == user.OrganizationUnit;
        }
      }

      if (AuthorizationRequirement.Delete.Equals(requirement))
      {
        if (user.IsContainsAllRights(Right.WriteEmployeeAll))
        {
          return x => true;
        }

        if (user.IsContainsAllRights(Right.WriteEmployeeInBL))
        {
          return x => x.OrganizationUnit == user.OrganizationUnit;
        }
      }

      return base.GetExpressionAuthorizationRequirement(user, requirement);
    }
  }
}