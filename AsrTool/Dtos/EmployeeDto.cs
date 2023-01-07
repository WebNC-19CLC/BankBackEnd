﻿namespace AsrTool.Dtos
{
  public class EmployeeDto
  {
    public int? Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Address { get; set; }
    public bool IsActive { get; set; }
  }
}
