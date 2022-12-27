namespace AsrTool.Dtos
{
  public class AccountDto
  {
    public int Id { get; set; }
    public string AccountNumber { get; set; }
    public string Owner { get; set; }
    public double Balance { get; set; }
    public ICollection<RecipientDto> Recipients { get; set; } = new List<RecipientDto>();
  }
}
