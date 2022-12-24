namespace AsrTool.Dtos.UserRoleDtos
{
  public class SearchUsersByTermFilterDto : BaseExcludeFilterDto
  {
    public ICollection<int> ExcludedRoleIds { get; set; }
  }
}
