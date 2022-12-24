namespace AsrTool.Infrastructure.Domain.Entities.Interfaces
{
  public interface IAuditing
  {
    DateTime CreatedOn { get; set; }

    string CreatedBy { get; set; }

    DateTime ModifiedOn { get; set; }

    string ModifiedBy { get; set; }
  }
}
