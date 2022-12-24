using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsrTool.Infrastructure.Context.EntityConfigurations
{
  public class AccountEntityConfig : IEntityTypeConfiguration<Account>
  {
    public void Configure(EntityTypeBuilder<Account> builder)
    {
      builder.HasIndex(x => x.AccountNumber).IsUnique();
    }
  }
}
