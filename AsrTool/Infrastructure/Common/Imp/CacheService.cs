using System.Security;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Domain.Objects.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AsrTool.Infrastructure.Common.Imp
{
  public class CacheService : ICacheService
  {
    private readonly ILogger<CacheService> _logger;
    private readonly IAsrContext _context;
    private readonly IMemoryCache _cache;

    public CacheService(ILogger<CacheService> logger, IAsrContext context, IMemoryCache cache)
    {
      _logger = logger;
      _context = context;
      _cache = cache;
    }

    public async Task<EmployeeCachingItem> GetEmployeeCachingItem(string username)
    {
      try
      {
        return await _cache.GetOrCreate(GetAuthenticatedUserCacheKey(username), async (cacheEntry) =>
        {
          cacheEntry.Priority = CacheItemPriority.Normal;
          cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(Constants.Caching.CACHING_ITEM_TIME_SECOND);
          return await GetEmployee(username);
        });
      }
      catch (Exception)
      {
        RemoveAuthenticatedUserCache(username);
        throw;
      }
    }

    public bool RemoveAuthenticatedUserCache(string userName)
    {
      try
      {
        _cache.Remove(GetAuthenticatedUserCacheKey(userName));
        return true;
      }
      catch (Exception exception)
      {
        _logger.LogError(exception, $"Can not remove AuthenticatedUser cache of user {userName}");
        return false;
      }
    }

    private async Task<EmployeeCachingItem> GetEmployee(string username)
    {
      var employee = await _context.Get<Employee>().Include(x => x.Role)
        .SingleOrDefaultAsync(x => x.Username == username);

      if (employee == default)
      {
        throw new SecurityException("Forbidden");
      }

      return new EmployeeCachingItem
      {
        Id = employee.Id,
        Username = employee.Username,
        Rights = employee.Role?.Rights,
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Visa = employee.Visa,
        Email = employee.Email,
        Level = employee.Level,
        LegalUnit = employee.LegalUnit,
        Site = employee.Site,
        OrganizationUnit = employee.OrganizationUnit,
        Department = employee.Department,
        RoleName = employee.Role?.Name,
        TimeZoneId = employee.TimeZoneId
      };
    }

    private static string GetAuthenticatedUserCacheKey(string userName)
    {
      return $"{Constants.Caching.USER_CACHE}-{userName}".ToUpperInvariant();
    }
  }
}
