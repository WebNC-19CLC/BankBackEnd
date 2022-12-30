using AsrTool.Infrastructure.Domain.Entities.Interfaces;

namespace AsrTool.Infrastructure.Domain.Entities
{
  public class Debit : IIdentity, IVersioning, IAuditing
  {
    public int Id { get; set; }

    public byte[]? RowVersion { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = default!;

    public DateTime ModifiedOn { get; set; }

    public string ModifiedBy { get; set; } = default!;

    public int? FromId { get; set; }

    public BankAccount? From { get; set; }

    public int? ToId { get; set; }

    public BankAccount? To { get; set; }

    public string? ToAccountNumber { get; set; }

    public int? BankDestinationId { get; set; }

    public Bank? BankDestination { get; set; }

    public string? FromAccountNumber { get; set; }

    public double Amount { get; set; }

    public string Description { get; set; }
  }
}
