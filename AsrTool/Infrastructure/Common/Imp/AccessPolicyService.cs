using System.Linq.Expressions;
using System.Security;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Common.Policy;
using AsrTool.Infrastructure.Domain.Entities.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AsrTool.Infrastructure.Common.Imp
{
  public class AccessPolicyService : IAccessPolicyService
  {
    private readonly IEnumerable<IAuthorizationHandler> _handlers;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserResolver _userResolver;

    public AccessPolicyService(IAuthorizationService authorizationService, IUserResolver userResolver, IEnumerable<IAuthorizationHandler> handlers)
    {
      _authorizationService = authorizationService;
      _userResolver = userResolver;
      _handlers = handlers;
    }

    public async Task EnsureAccessPolicy<T>(T entity, params AuthorizationRequirement[] requirements)
        where T : IIdentity
    {
      if (!await HasAccessPolicy(entity, requirements))
      {
        throw new SecurityException(
            $"Forbidden: {entity.GetType().Name}, id {entity.Id} with requirement(s) '{string.Join(", ", requirements.Select(x => x.Name))}' can not access");
      }
    }

    public async Task<bool> HasAccessPolicy<T>(T entity, params AuthorizationRequirement[] requirements)
        where T : IIdentity
    {
      var result = await _authorizationService
        .AuthorizeAsync(_userResolver.CurrentUser.Principal, entity, requirements);
      return result.Succeeded;
    }

    public Expression<Func<T, bool>> CanAccess<T>(AuthorizationRequirement requirement)
      where T : IIdentity
    {
      var handler = _handlers.Where(x => x is AuthorizationHandlerBase<T>).Cast<AuthorizationHandlerBase<T>>().SingleOrDefault();
      if (handler == null)
      {
        throw new NotSupportedException();
      }

      return handler.GetExpressionAuthorizationRequirement(_userResolver.CurrentUser, requirement);
    }
  }
}