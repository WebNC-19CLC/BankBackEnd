using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsrTool.Infrastructure.Context.EntityConfigurations
{
  public class AccountEntityConfig : IEntityTypeConfiguration<BankAccount>
  {
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
      builder.HasIndex(x => x.AccountNumber).IsUnique();
    }
  }
}
