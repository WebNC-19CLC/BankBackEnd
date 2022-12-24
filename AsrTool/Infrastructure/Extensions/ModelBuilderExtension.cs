using AsrTool.Infrastructure.Domain.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.Extensions
{
  public static class ModelBuilderExtension
  {
    public static void ConfigTableName(this ModelBuilder builder)
    {
      var clrTypes = GetClrTypes(builder);
      UseLog(nameof(ConfigTableName), clrTypes, clrTypes);
      foreach (var clrType in clrTypes)
      {
        builder.Entity(clrType).ToTable(clrType.Name);
      }
    }

    public static void ConfigIdentity(this ModelBuilder builder)
    {
      var clrTypes = GetClrTypes(builder);
      var appliedOnClrTypes = clrTypes.Where(x => !clrTypes.Any(x.IsSubclassOf)).ToArray();
      UseLog(nameof(ConfigIdentity), clrTypes, appliedOnClrTypes);
      foreach (var clrType in appliedOnClrTypes)
      {
        builder.Entity(clrType).HasKey(nameof(IIdentity.Id));
        builder.Entity(clrType).Property(nameof(IIdentity.Id)).ValueGeneratedOnAdd();
      }
    }

    public static void ConfigAudit(this ModelBuilder builder)
    {
      var clrTypes = GetClrTypes(builder);
      var appliedOnClrTypes = clrTypes
        .Where(x => x.IsAssignableTo(typeof(IAuditing)))
        .Where(x => !clrTypes.Any(t => x.IsSubclassOf(t) && t.IsAssignableTo(typeof(IAuditing))))
        .ToArray();
      UseLog(nameof(ConfigAudit), clrTypes, appliedOnClrTypes);
      foreach (var clrType in appliedOnClrTypes)
      {
        builder.Entity(clrType).Property(nameof(IAuditing.CreatedBy)).HasUnicodeTextColumn(50);
        builder.Entity(clrType).Property(nameof(IAuditing.ModifiedBy)).HasUnicodeTextColumn(50);
      }
    }

    public static void ConfigRowVersion(this ModelBuilder builder)
    {
      var clrTypes = GetClrTypes(builder);
      var appliedOnClrTypes = clrTypes
        .Where(x => x.IsAssignableTo(typeof(IVersioning)))
        .Where(x => !clrTypes.Any(t => x.IsSubclassOf(t) && t.IsAssignableTo(typeof(IVersioning))))
        .ToArray();
      UseLog(nameof(ConfigRowVersion), clrTypes, appliedOnClrTypes);
      foreach (var clrType in appliedOnClrTypes)
      {
        builder.Entity(clrType).Property(nameof(IVersioning.RowVersion)).IsRowVersion();
      }
    }

    private static ICollection<Type> GetClrTypes(ModelBuilder builder)
    {
      return builder.Model.GetEntityTypes().Where(x => x.ClrType.IsAssignableTo(typeof(IIdentity))).Select(x => x.ClrType).ToArray();
    }

    private static void UseLog(string methodName, ICollection<Type> clrTypes, ICollection<Type> appliedOnClrTypes)
    {
#if DEBUG
      // Used for EF-migration only
      Console.WriteLine($"{methodName} Types: {string.Join(", ", clrTypes.Select(x => x.Name))}");
      Console.WriteLine($"{methodName} AppliedTypes: {string.Join(", ", appliedOnClrTypes.Select(x => x.Name))}");
      Console.WriteLine($"{methodName} IgnoredTypes: {string.Join(", ", clrTypes.Except(appliedOnClrTypes).Select(x => x.Name))}");
      Console.WriteLine($"-------------------------------------------**---------------------------------------------------------");
#endif
    }
  }
}
