namespace AsrTool.Dtos
{
  public class SelfReceiveDto
  {
    public string FromAccountNumber { get; set; }
    public double Amount { get; set; }
    public int? BankId { get; set; }
    public bool ChargeReceiver { get; set; } = false;
  }
}
