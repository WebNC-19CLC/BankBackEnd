using AsrTool.Infrastructure.Domain.Entities.Interfaces;

namespace AsrTool.Infrastructure.Domain.Entities
{
  public class BankAccount : IIdentity, IVersioning, IAuditing
  {
    public int Id { get; set; }

    public byte[]? RowVersion { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = default!;

    public DateTime ModifiedOn { get; set; }

    public string ModifiedBy { get; set; } = default!;

    public string AccountNumber { get; set; }

    public double Balance { get; set; }

    public ICollection<Recipient> Recipients { get; set; } = new HashSet<Recipient>();

    public ICollection<Debit> Debits { get; set; } = new HashSet<Debit>();

    public int? OTPId { get; set; }

    public OTP? OTP { get; set; }

    public int? UserId { get; set; }

    public Employee? User { get; set; }
  }
}
