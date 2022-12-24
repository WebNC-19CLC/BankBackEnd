namespace AsrTool.Dtos
{
  public class DataSourceRequestDto
  {
    public int Take { get; set; } = 10;

    public int Skip { get; set; } = 0;

    public bool? PreloadAllData { get; set; } = false;

    public ICollection<SortFieldDto>? Sorts { get; set; } = new List<SortFieldDto>();
  }
}
