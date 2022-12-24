using System.Linq.Expressions;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Domain.Entities.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.Common.Policy
{
  public abstract class AuthorizationHandlerBase<TEntity> : AuthorizationHandler<AuthorizationRequirement, TEntity>
        where TEntity : IIdentity
  {
    public virtual Expression<Func<TEntity, bool>> GetExpressionAuthorizationRequirement(IUser user, AuthorizationRequirement requirement)
    {
      return x => false;
    }
  }
}