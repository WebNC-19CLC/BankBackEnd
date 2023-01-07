namespace AsrTool.Dtos
{
  public class MakeNotificationDto
  {
    public string Description { get; set; }
    public int AccountId { get; set; }
    public string Type { get; set; } = "Debit";
  }
}
