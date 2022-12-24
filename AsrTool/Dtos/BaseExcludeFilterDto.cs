namespace AsrTool.Dtos
{
  public class BaseExcludeFilterDto
  {
    public string SearchTerm { get; set; }
    public int[] ExcludedIds { get; set; }
  }
}
