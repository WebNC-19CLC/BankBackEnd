namespace AsrTool.Dtos
{
  public class SelfTransferDto
  {
    public string ToAccountNumber { get; set; }
    public double Amount { get; set; }
    public int? BankId { get; set; }
    public string Description { get; set; }
    public string OTP { get; set; }
    public bool ChargeReceiver { get; set; } = false;
  }
}
