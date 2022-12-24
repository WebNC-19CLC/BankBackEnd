namespace AsrTool.Dtos.UserRoleDtos
{
  public class UserRefDto
  {
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Visa { get; set; }
    public string FullNameAndVisa => $"{FullName} ({Visa})";
    public int RoleId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
