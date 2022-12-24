namespace AsrTool.Dtos
{
  public class DataSourceResultDto<T>
  {
    public int Total { get; set; }
    public ICollection<T> Data { get; set; } = new List<T>();
  }
}
