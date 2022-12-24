namespace AsrTool.Dtos.UserRoleDtos
{
  public class AssignUsersToRoleRequestDto
  {
    public bool? RemoveCurrentRole { get; set; }
    public int? RoleId { get; set; }
    public ICollection<int> UserIds { get; set; }
  }
};
