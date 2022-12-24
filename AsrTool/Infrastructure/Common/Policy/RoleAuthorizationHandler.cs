using System.Linq.Expressions;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.Common.Policy
{
  public class RoleAuthorizationHandler : AuthorizationHandlerBase<Role>
  {
    private readonly IUserResolver _userResolver;

    public RoleAuthorizationHandler(IUserResolver userResolver)
    {
      _userResolver = userResolver;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement, Role resource)
    {
      if (resource == null)
      {
        return Task.CompletedTask;
      }

      var validationFunc = GetExpressionAuthorizationRequirement((IUser)_userResolver.CurrentUser, requirement).Compile();
      if (validationFunc(resource))
      {
        context.Succeed(requirement);
        return Task.CompletedTask;
      }

      return Task.CompletedTask;
    }

    public override Expression<Func<Role, bool>> GetExpressionAuthorizationRequirement(IUser user, AuthorizationRequirement requirement)
    {
      if (AuthorizationRequirement.Create.Equals(requirement))
      {
        if (user.IsContainsAnyRights(Right.WriteRole, Right.WriteRoleAll))
        {
          return x => true;
        }
      }

      if (AuthorizationRequirement.Read.Equals(requirement))
      {
        if (user.IsContainsAllRights(Right.ReadRoleAll))
        {
          return x => true;
        }

        if (user.IsContainsAllRights(Right.ReadRole))
        {
          return x => x.CreatedBy == user.Username;
        }
      }

      if (AuthorizationRequirement.Update.Equals(requirement))
      {
        if (user.IsContainsAllRights(Right.WriteRoleAll))
        {
          return x => true;
        }

        if (user.IsContainsAllRights(Right.WriteRole))
        {
          return x => x.CreatedBy == user.Username;
        }
      }

      if (AuthorizationRequirement.Delete.Equals(requirement))
      {
        if (user.IsContainsAllRights(Right.WriteRoleAll))
        {
          return x => true;
        }

        if (user.IsContainsAllRights(Right.WriteRole))
        {
          return x => x.CreatedBy == user.Username;
        }
      }

      if (AuthorizationRequirement.ResetRights.Equals(requirement))
      {
        if (user.IsContainsAnyRights(Right.WriteRoleAll, Right.ResetRights))
        {
          return x => true;
        }

        if (user.IsContainsAllRights(Right.WriteRole))
        {
          return x => x.CreatedBy == user.Username;
        }
      }

      return base.GetExpressionAuthorizationRequirement(user, requirement);
    }
  }
}