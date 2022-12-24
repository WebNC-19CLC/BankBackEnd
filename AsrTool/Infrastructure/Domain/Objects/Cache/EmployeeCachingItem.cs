using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.Infrastructure.Domain.Objects.Cache
{
  public class EmployeeCachingItem
  {
    public int Id { get; set; }

    public string Username { get; set; } = default!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Visa { get; set; } = default!;

    public string? Email { get; set; }

    public string? LegalUnit { get; set; }

    public string? Site { get; set; }

    public string? OrganizationUnit { get; set; }

    public string? Department { get; set; }

    public int? Level { get; set; }

    public ICollection<Right>? Rights { get; set; }

    public string? RoleName { get; set; }

    public string? TimeZoneId { get; set; }
  }
}
