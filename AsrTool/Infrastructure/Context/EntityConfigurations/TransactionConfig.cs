using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsrTool.Infrastructure.Context.EntityConfigurations
{
  public class TransactionConfig : IEntityTypeConfiguration<Transaction>
  {
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
      builder.HasOne(x => x.From).WithMany().HasForeignKey(x => x.FromId).OnDelete(DeleteBehavior.ClientSetNull);
      builder.HasOne(x => x.To).WithMany().HasForeignKey(x => x.ToId).OnDelete(DeleteBehavior.ClientSetNull);
      builder.HasOne(x => x.BankDestination).WithMany().HasForeignKey(x => x.BankDestinationId).OnDelete(DeleteBehavior.ClientSetNull);
      builder.HasOne(x => x.BankSource).WithMany().HasForeignKey(x => x.BankSourceId).OnDelete(DeleteBehavior.ClientSetNull);
    }
  }
}
