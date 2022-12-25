using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsrTool.Infrastructure.Context.EntityConfigurations
{
  public class RecipientEntityConfig : IEntityTypeConfiguration<Recipient>
  {
    public void Configure(EntityTypeBuilder<Recipient> builder)
    {
      builder.HasIndex(x => x.AccountNumber).IsUnique();
      builder.HasOne(x => x.BankDestination).WithMany().HasForeignKey(x => x.BankDestinationId).OnDelete(DeleteBehavior.ClientSetNull);
    }
  }
}
