namespace AsrTool.Dtos
{
  public class TransactionDto
  {
    public int Id { get; set; }
    public string FromAccountNumber { get; set; }
    public string ToAccountNumber { get; set; }
    public int? BankDestinationId { get; set; }
    public double Amount { get; set; }
    public DateTime Time { get; set; }
  }
}
