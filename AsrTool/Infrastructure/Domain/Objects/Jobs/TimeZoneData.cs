namespace AsrTool.Infrastructure.Domain.Objects.Jobs
{
  public class TimeZoneData
  {
    public string TimeZoneId { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string? Description { get; set; }
  }
}
