namespace AsrTool.Dtos
{
  public class SearchAccountRequestDto
  {
    public string AccountNumber { get; set; }

    public int? BankId { get; set; } = null;
  }
}
