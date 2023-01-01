namespace AsrTool.Dtos
{
  public class CreateDebitDto
  {
    public string AccountNumber { get; set; }

    public bool SelfInDebt { get; set; }

    public double Amount { get; set; }

    public string? Description { get; set; }

    public DateTime DateDue { get; set; }

    public int? BankDestinationId { get; set; }
  }
}
