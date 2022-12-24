using AsrTool.Infrastructure.Domain.Entities.Interfaces;

namespace AsrTool.Infrastructure.Domain.Entities
{
  public class Transaction : IVersioning, IAuditing, IIdentity
  {
    public int Id { get; set; }

    public byte[]? RowVersion { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = default!;

    public DateTime ModifiedOn { get; set; }

    public string ModifiedBy { get; set; } = default!;

    public int FromId { get; set; }

    public Account From { get; set; }

    public int ToId { get; set; }

    public Account To { get; set; }

    public double Amount { get; set; }
  }
}
