using AsrTool.Infrastructure.Domain.Entities.Interfaces;

namespace AsrTool.Infrastructure.Domain.Entities
{
  public class Notification : IIdentity, IVersioning, IAuditing
  {
    public int Id { get; set; }

    public byte[]? RowVersion { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = default!;

    public DateTime ModifiedOn { get; set; }

    public string ModifiedBy { get; set; } = default!;

    public string Description { get; set; }

    public int BankAccountId { get; set; }

    public BankAccount BankAccount { get; set; }
  }
}
