using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.Dtos
{
  public class SortFieldDto
  {
    public string Field { get; set; }

    public SortOrder Order { get; set; } = SortOrder.Asc;
  }
}
