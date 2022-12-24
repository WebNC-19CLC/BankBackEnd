namespace AsrTool.Dtos
{
  public class DataSourceRequestFilterDto<T>
  {
    public DataSourceRequestDto DataSourceRequest { get; set; } = new DataSourceRequestDto();
    public T Filter { get; set; }
  }
}
