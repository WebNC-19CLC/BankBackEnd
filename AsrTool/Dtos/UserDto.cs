using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.Dtos
{
  public class UserDto
  {
    public int EmployeeId { get; set; }

    public string IdentityName { get; set; } = default!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FullName { get; set; }

    public string Visa { get; set; } = default!;

    public string ImageSmall => string.Format(Constants.Avatar.AVATAR_SMALL_URL_TEMPLATE, Visa);

    public string ImageLarge => string.Format(Constants.Avatar.AVATAR_LARGE_URL_TEMPLATE, Visa);

    public IEnumerable<Right> Rights { get; set; } = Array.Empty<Right>();

    public string? LegalUnit { get; set; }

    public string? Site { get; set; }

    public string? OrganizationUnit { get; set; }

    public double? Level { get; set; }

    public string? AuthType { get; set; }

    public string? XsrfToken { get; set; }

    public string RoleName { get; set; } = default!;

    public string? TimeZoneId { get; set; }
  }
}
