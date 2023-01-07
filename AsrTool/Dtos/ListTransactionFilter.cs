namespace AsrTool.Dtos
{
  public class ListTransactionFilter
  { 
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public string? Type { get; set; }
    public int? BankDestinationId { get; set; }
    public int? BankSourceId { get; set; }
  }
}
