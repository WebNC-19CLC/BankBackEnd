using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.Dtos
{
  public class RoleDto
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<Right> Rights { get; set; } = new HashSet<Right>();

    public byte[] RowVersion { get; set; }

  }
}