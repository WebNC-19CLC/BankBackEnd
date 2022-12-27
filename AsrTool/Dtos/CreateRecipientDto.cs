namespace AsrTool.Dtos
{
  public class CreateRecipientDto
  {
    public string AccountNumber { get; set; }

    public string SuggestedName { get; set; }

    public int? BankDestinationId { get; set; }
  }
}
