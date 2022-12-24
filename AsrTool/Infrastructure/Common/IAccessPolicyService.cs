using System.Linq.Expressions;
using AsrTool.Infrastructure.Common.Policy;
using AsrTool.Infrastructure.Domain.Entities.Interfaces;

namespace AsrTool.Infrastructure.Common
{
  public interface IAccessPolicyService
  {
    Task EnsureAccessPolicy<T>(T entity, params AuthorizationRequirement[] requirements) where T : IIdentity;

    Task<bool> HasAccessPolicy<T>(T entity, params AuthorizationRequirement[] requirements) where T : IIdentity;

    Expression<Func<T, bool>> CanAccess<T>(AuthorizationRequirement requirement) where T : IIdentity;
  }
}