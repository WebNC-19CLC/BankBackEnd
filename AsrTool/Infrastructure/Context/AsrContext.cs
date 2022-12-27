using System.Reflection;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Domain.Entities.Interfaces;
using AsrTool.Infrastructure.Extensions;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

namespace AsrTool.Infrastructure.Context
{
  public class AsrContext : DbContext, IAsrContext
  {
    private readonly IUserResolver _userResolver;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public AsrContext(DbContextOptions options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public AsrContext(DbContextOptions options, IUserResolver userResolver) : base(options)
    {
      _userResolver = userResolver;
    }

    protected DbSet<Employee> Employees { get; set; } = default!;

    protected DbSet<Role> Roles { get; set; } = default!;

    protected DbSet<BankAccount> BankAccounts { get; set; } = default!;

    protected DbSet<Transaction> Transactions { get; set; } = default!;

    protected DbSet<Debit> Debits { get; set; } = default!;

    protected DbSet<Recipient> Recipients { get; set; }

    protected DbSet<Bank> Banks { get; set; } = default!;

    protected DbSet<OTP> OTPs { get; set; }
     
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.ConfigIdentity();
      modelBuilder.ConfigAudit();
      modelBuilder.ConfigRowVersion();
      modelBuilder.ConfigTableName();

      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public IQueryable<T> Get<T>() where T : class, IIdentity
    {
      return base.Set<T>().AsNoTracking();
    }

    public override DbSet<TEntity> Set<TEntity>()
    {
      throw new NotSupportedException("Please use Get instead");
    }

    public override DbSet<TEntity> Set<TEntity>(string name)
    {
      throw new NotSupportedException("Please use Get instead");
    }

    public override async ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = new CancellationToken())
      where TEntity : class
    {
      return await base.AddAsync(entity, cancellationToken);
    }

    public override async ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = new CancellationToken())
    {
      return await base.AddAsync(entity, cancellationToken);
    }

    public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
    {
      throw new NotSupportedException("Please use AddAsync instead");
    }

    public override EntityEntry Add(object entity)
    {
      throw new NotSupportedException("Please use AddAsync instead");
    }

    public override async Task AddRangeAsync(params object[] entities)
    {
      await base.AddRangeAsync(entities);
    }

    public override async Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = new CancellationToken())
    {
      await base.AddRangeAsync(entities, cancellationToken);
    }

    public override void AddRange(params object[] entities)
    {
      throw new NotSupportedException("Please use AddRangeAsync instead");
    }

    public override void AddRange(IEnumerable<object> entities)
    {
      throw new NotSupportedException("Please use AddRangeAsync instead");
    }

    public Task<EntityEntry<T>> UpdateAsync<T>(T entity) where T : class, IIdentity
    {
      return Task.FromResult(base.Update(entity));
    }

    public Task<EntityEntry> UpdateAsync(object entity)
    {
      return Task.FromResult(base.Update(entity));
    }

    public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
    {
      throw new NotSupportedException("Please use UpdateAsync instead");
    }

    public override EntityEntry Update(object entity)
    {
      throw new NotSupportedException("Please use UpdateAsync instead");
    }

    public Task UpdateRangeAsync<T>(IEnumerable<T> entities) where T : class, IIdentity
    {
      base.UpdateRange(entities);
      return Task.CompletedTask;
    }

    public override void UpdateRange(params object[] entities)
    {
      throw new NotSupportedException("Please use UpdateRangeAsync instead");
    }

    public override void UpdateRange(IEnumerable<object> entities)
    {
      throw new NotSupportedException("Please use UpdateRangeAsync instead");
    }

    public Task<EntityEntry<T>> RemoveAsync<T>(T entity) where T : class, IIdentity
    {
      return Task.FromResult(base.Remove(entity));
    }

    public Task<EntityEntry> RemoveAsync(object entity)
    {
      return Task.FromResult(base.Remove(entity));
    }

    public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
    {
      throw new NotSupportedException("Please use RemoveAsync instead");
    }

    public override EntityEntry Remove(object entity)
    {
      throw new NotSupportedException("Please use RemoveAsync instead");
    }

    public Task RemoveRangeAsync<T>(IEnumerable<T> entities) where T : class, IIdentity
    {
      base.RemoveRange(entities);
      return Task.CompletedTask;
    }

    public override void RemoveRange(params object[] entities)
    {
      throw new NotSupportedException("Please use RemoveRangeAsync instead");
    }

    public override void RemoveRange(IEnumerable<object> entities)
    {
      throw new NotSupportedException("Please use RemoveRangeAsync instead");
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken(), bool auditForAddOnly = false)
    {
      await SetAuditData(auditForAddOnly);
      await AdaptRowVersion();
      return await base.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SaveChangesNoAuditAsync(CancellationToken cancellationToken = new CancellationToken())
    {
      await AdaptRowVersion();
      return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
      throw new NotSupportedException("Please use SaveChangesAsync instead");
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
      throw new NotSupportedException("Please use SaveChangesAsync instead");
    }

    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
      return Database.BeginTransactionAsync();
    }

    public async Task BulkInsertAsync<T>(IEnumerable<T> entities) where T : class, IIdentity
    {
      var entitiesArray = entities.ToArray();
      await AddAuditData(entitiesArray.Where(x => x is IAuditing).Cast<IAuditing>().Select(x => (x, EntityState.Added)));
      await DbContextBulkExtensions.BulkInsertAsync(this, entitiesArray);
    }

    public async Task MigrateAsync(string? targetMigration = null)
    {
      var migrator = Database.GetService<IMigrator>();
      await migrator.MigrateAsync(targetMigration);
    }

    public Task ClearChangeTracker()
    {
      ChangeTracker.Clear();
      return Task.CompletedTask;
    }

    public new EntityEntry<T> Entry<T>(T entity) where T : class, IIdentity
    {
      return base.Entry(entity);
    }

    public IEnumerable<EntityEntry<T>> Entries<T>() where T : class, IIdentity
    {
      return ChangeTracker.Entries<T>();
    }

    private Task AdaptRowVersion()
    {
      var auditEntries = ChangeTracker.Entries<IVersioning>();
      foreach (var entityEntry in auditEntries.Where(x => x?.Entity != null && x.State == EntityState.Modified))
      {
        entityEntry.OriginalValues[nameof(IVersioning.RowVersion)] = entityEntry.Entity.RowVersion;
      }
      return Task.CompletedTask;
    }

    private async Task SetAuditData(bool auditForAddOnly = false)
    {
      var entries = ChangeTracker.Entries<IAuditing>();

      if (auditForAddOnly)
      {
        entries = entries.Where(x => x.State == EntityState.Added);
      }

      await AddAuditData(entries.Select(x => (x.Entity, x.State)));
    }

    private Task AddAuditData(IEnumerable<(IAuditing Entity, EntityState State)> entities)
    {
      var now = DateTime.UtcNow;
      foreach (var entityEntry in entities)
      {
        var entity = entityEntry.Entity;
        if (entityEntry.State == EntityState.Added)
        {
          entity.CreatedBy = _userResolver.CurrentUser.Username != null ? _userResolver.CurrentUser.Username : "SYSTEM";
          entity.CreatedOn = now;
          entity.ModifiedBy = entity.CreatedBy;
          entity.ModifiedOn = entity.CreatedOn;
        }
        else if (entityEntry.State == EntityState.Modified)
        {
          entity.ModifiedBy = _userResolver.CurrentUser.Username;
          entity.ModifiedOn = now;
        }
      }

      return Task.CompletedTask;
    }

    public bool IsDatabaseExist => (Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();
  }
}
