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
      builder.HasOne(x => x.OTP).WithMany().HasForeignKey(x => x.OTPId).OnDelete(DeleteBehavior.ClientSetNull);
      builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.ClientSetNull);
    }
  }
}
