using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsrTool.Infrastructure.Context.EntityConfigurations
{
  public class RoleEntityConfig : IEntityTypeConfiguration<Role>
  {
    public void Configure(EntityTypeBuilder<Role> builder)
    {
      builder.HasIndex(x => x.Name).IsUnique();
      builder.Property(x => x.Name).HasAsciiColumn(128).IsRequired();
      builder.Property(x => x.Rights).HasEnumCollectionToStringConversion().HasMaxAsciiColumn().IsRequired();
    }
  }
}
