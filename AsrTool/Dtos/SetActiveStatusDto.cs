namespace AsrTool.Dtos
{
  public class SetActiveStatusDto
  {
    public int BankAccountId { get; set; }

    public bool IsActive { get; set; } = true;
  }
}
