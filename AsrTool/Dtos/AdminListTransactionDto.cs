namespace AsrTool.Dtos
{
  public class AdminListTransactionDto
  {
    public double? TotalAmount { get; set; }
    public ICollection<TransactionDto> TransactionList { get; set; } = new List<TransactionDto>();
  }
}
