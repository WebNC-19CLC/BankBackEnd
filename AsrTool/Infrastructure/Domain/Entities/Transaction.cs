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

    public BankAccount From { get; set; }

    public int? ToId { get; set; }

    public BankAccount? To { get; set; }

    public double Amount { get; set; }

    public string Type { get; set; }

    public string Description { get; set; }

    public bool Status { get; set; } = false;

    public string? ToAccountNumber { get; set; }

    public int? BankDestinationId { get; set; }

    public bool ChargeReceiver { get; set; } = false;

    public double? TransactionFee { get; set; }

    public Bank? BankDestination { get; set; }
  }
}
