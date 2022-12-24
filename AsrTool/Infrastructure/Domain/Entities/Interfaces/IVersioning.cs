namespace AsrTool.Infrastructure.Domain.Entities.Interfaces
{
  public interface IVersioning
  {
    byte[]? RowVersion { get; set; }
  }
}
