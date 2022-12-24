namespace AsrTool.Dtos.UserRoleDtos
{
  public class GetUsersByRolesFilterDto
  {
    public DataSourceRequestDto Request { get; set; }
    public ICollection<int> RoleIds { get; set; }
  }
}
