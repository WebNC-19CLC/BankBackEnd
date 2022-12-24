using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsrTool.Infrastructure.Context.EntityConfigurations
{
  public class EmployeeEntityConfig : IEntityTypeConfiguration<Employee>
  {
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
      builder.Property(x => x.FirstName).HasUnicodeTextColumn(256);
      builder.Property(x => x.LastName).HasUnicodeTextColumn(256);
      builder.HasIndex(x => x.Visa).IsUnique().HasFilter($"{nameof(Employee.Active)} = 1");
      builder.Property(x => x.Visa).HasAsciiColumn(16).IsRequired();
      builder.Property(x => x.Gender).HasEnumToStringConversation().HasAsciiColumn(64).IsRequired();
      builder.Property(x => x.Email).HasUnicodeTextColumn(256);
      builder.Property(x => x.HeadUnitVisa).HasAsciiColumn(16);
      builder.Property(x => x.HeadOperationVisa).HasAsciiColumn(16);
      builder.Property(x => x.AdDomain).HasUnicodeTextColumn(64);
      builder.Property(x => x.OrganizationUnit).HasUnicodeTextColumn(256);
      builder.Property(x => x.Department).HasUnicodeTextColumn(256);
      builder.HasIndex(x => x.Username).IsUnique();
      builder.Property(x => x.Username).HasAsciiColumn(128).IsRequired();
      builder.Property(x => x.Site).HasUnicodeTextColumn(256);
      builder.Property(x => x.TechnicalRole).HasUnicodeTextColumn(256);
      builder.Property(x => x.LegalUnit).HasUnicodeTextColumn(256);
      builder.Property(x => x.TimeZoneId).IsRequired().HasDefaultValue(Constants.TimeZoneId.DEFAULT);
      builder.HasOne(x => x.Supervisor).WithMany().HasForeignKey(x => x.SupervisorId).OnDelete(DeleteBehavior.ClientSetNull);
    }
  }
}
