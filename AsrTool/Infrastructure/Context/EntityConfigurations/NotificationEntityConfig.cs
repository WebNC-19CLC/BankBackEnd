using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsrTool.Infrastructure.Context.EntityConfigurations
{
  public class NotificationEntityConfig : IEntityTypeConfiguration<Notification>
  {
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
    }
  }
}
