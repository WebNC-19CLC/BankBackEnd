namespace AsrTool.Dtos.UserRoleDtos
{
  public class UserByRoleDto
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string FullName { get; set; }
    public string Visa { get; set; }
    public string AvatarUrl => string.Format(Constants.Avatar.AVATAR_SMALL_URL_TEMPLATE, Visa);
    public string TechnicalRole { get; set; }
    public int? Level { get; set; }
    public string LegalUnit { get; set; }
    public string WorkLocation { get; set; }
    public int RoleId { get; set; }
  }
}
