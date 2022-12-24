namespace AsrTool.Dtos
{
  public class TransactionDto
  {
    public int Id { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public double Amount { get; set; }
    public DateTime Time { get; set; }
  }
}
