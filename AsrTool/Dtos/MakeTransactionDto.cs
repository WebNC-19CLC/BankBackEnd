namespace AsrTool.Dtos
{
  public class MakeTransactionDto
  {
    public string From { get; set; }
    public string To { get; set; }
    public double Amount { get; set; }
    public int? BankId { get; set; }
    public bool ChargeReceiver { get; set; }
  }
}
