using AsrTool.UnitTest._Common.Fake;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities.Interfaces;
using AsrTool.Infrastructure.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AsrTool.UnitTest._Common.Extensions
{
  internal static class ServiceProviderExtensions
  {
    internal static IQueryable<T> GetEntities<T>(this IServiceProvider provider)
      where T : class, IIdentity
    {
      var context = provider.Resolve<IAsrContext>();
      return context.Get<T>();
    }

    internal static async Task<IServiceProvider> AddEntitiesAsync<T>(this IServiceProvider provider, params T[] entity)
      where T : class, IIdentity
    {
      var context = provider.Resolve<IAsrContext>();
      await context.AddRangeAsync(entity);
      return provider;
    }

    internal static async Task<IServiceProvider> SaveChangesAsync(this IServiceProvider provider)
    {
      var context = provider.Resolve<IAsrContext>();
      await context.SaveChangesAsync();
      return provider;
    }

    internal static async Task<IServiceProvider> SaveChangesAsync(this IServiceProvider provider, string username)
    {
      var context = provider.Resolve<IAsrContext>();
      var userResolver = provider.Resolve<IUserResolver>();
      var unitTestUser = userResolver.CurrentUser as FakeUnitTestUser;
      var oldUsername = unitTestUser.Username;
      try
      {
        unitTestUser.Username = username;
        await context.SaveChangesAsync();
      }
      finally
      {
        unitTestUser.Username = oldUsername;
      }

      return provider;
    }

    internal static async Task<IServiceProvider> SaveChangesNoAuditAsync(this IServiceProvider provider)
    {
      var context = provider.Resolve<IAsrContext>();
      await context.SaveChangesNoAuditAsync();
      return provider;
    }

    internal static Task ClearChangesAsync(this IServiceProvider provider)
    {
      var context = provider.Resolve<IAsrContext>();
      return context.ClearChangeTracker();
    }

    internal static T Resolve<T>(this IServiceProvider provider)
    {
      var service = provider.GetService<T>();
      if (service == null)
      {
        throw new NotFoundException(
          $"Service {typeof(T).Name} is not found in container. Please inject it through override 'CustomRegister' or in 'BaseTest' class");
      }

      return service;
    }
  }
}