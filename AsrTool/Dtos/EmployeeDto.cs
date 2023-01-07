namespace AsrTool.Dtos
{
  public class EmployeeDto
  {
    public int? Id { get; set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string Address { get; set; }

    public string IndentityNumber { get; set; }

    public bool IsActive { get; set; }

    public string Role { get; set; }
  }
}
