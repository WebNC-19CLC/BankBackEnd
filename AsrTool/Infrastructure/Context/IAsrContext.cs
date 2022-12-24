using AsrTool.Infrastructure.Domain.Entities.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace AsrTool.Infrastructure.Context
{
  public interface IAsrContext
  {
    IQueryable<T> Get<T>() where T : class, IIdentity;

    ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = new CancellationToken()) where TEntity : class;

    ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = new CancellationToken());

    Task AddRangeAsync(params object[] entities);

    Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = new CancellationToken());

    Task<EntityEntry<T>> UpdateAsync<T>(T entity) where T : class, IIdentity;

    Task<EntityEntry> UpdateAsync(object entity);

    Task UpdateRangeAsync<T>(IEnumerable<T> entities) where T : class, IIdentity;

    Task<EntityEntry<T>> RemoveAsync<T>(T entity) where T : class, IIdentity;

    Task<EntityEntry> RemoveAsync(object entity);

    Task RemoveRangeAsync<T>(IEnumerable<T> entities) where T : class, IIdentity;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken(), bool auditForAddOnly = false);

    Task<int> SaveChangesNoAuditAsync(CancellationToken cancellationToken = new CancellationToken());

    Task<IDbContextTransaction> BeginTransactionAsync();

    Task BulkInsertAsync<T>(IEnumerable<T> entities) where T : class, IIdentity;

    Task MigrateAsync(string? targetMigration = null);

    Task ClearChangeTracker();

    IEnumerable<EntityEntry<T>> Entries<T>() where T : class, IIdentity;

    bool IsDatabaseExist { get; }
  }
}
