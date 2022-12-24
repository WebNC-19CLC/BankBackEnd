using AsrTool.Infrastructure.Domain.Entities.Interfaces;
using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.Infrastructure.Domain.Entities
{
  public class Role : IIdentity, IVersioning, IAuditing
  {
    public int Id { get; set; }

    public byte[]? RowVersion { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = default!;

    public DateTime ModifiedOn { get; set; }

    public string ModifiedBy { get; set; } = default!;

    public string Name { get; set; } = default!;

    public ICollection<Right> Rights { get; set; } = new HashSet<Right>();
  }
}
