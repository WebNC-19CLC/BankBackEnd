using AsrTool.Infrastructure.Domain.Objects.Cache;

namespace AsrTool.Infrastructure.Common
{
  public interface ICacheService
  {
    Task<EmployeeCachingItem> GetEmployeeCachingItem(string username);

    bool RemoveAuthenticatedUserCache(string userName);
  }
}