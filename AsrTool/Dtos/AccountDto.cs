namespace AsrTool.Dtos
{
  public class AccountDto
  {
    public int Id { get; set; }
    public string AccountNumber { get; set; }
    public string FullName { get; set; }
    public double Balance { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
    public ICollection<RecipientDto> Recipients { get; set; } = new List<RecipientDto>();
  }
}
