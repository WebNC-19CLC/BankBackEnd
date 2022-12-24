using AsrTool.Infrastructure.Domain.Entities.Interfaces;

namespace AsrTool.Infrastructure.Helpers
{
  public static class EntityHelper
  {
    public static void AdaptAuditableData<T>(T src, T dest) where T : IAuditing
    {
      dest.CreatedBy = src.CreatedBy;
      dest.CreatedOn = src.CreatedOn;
      dest.ModifiedBy = src.ModifiedBy;
      dest.ModifiedOn = src.ModifiedOn;
    }
  }
}