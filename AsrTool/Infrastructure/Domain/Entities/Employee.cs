using System.ComponentModel.DataAnnotations.Schema;
using AsrTool.Infrastructure.Domain.Entities.Interfaces;
using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.Infrastructure.Domain.Entities
{
  public class Employee : IIdentity, IVersioning, IAuditing
  {
    public int Id { get; set; }

    public byte[]? RowVersion { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = default!;

    public DateTime ModifiedOn { get; set; }

    public string ModifiedBy { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public int UniqueId { get; set; } = default!;

    public string? AdDomain { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public Gender Gender { get; set; }

    public DateTime? BirthDate { get; set; } // Sometimes, the birthdate is null in ERP system

    public string Visa { get; set; } = default!;

    public string? Email { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? DiplomaDate { get; set; }

    public string? LegalUnit { get; set; }

    public string? Site { get; set; }

    public string? OrganizationUnit { get; set; }

    public string? Department { get; set; }

    public string? HeadUnitVisa { get; set; }

    public string? HeadOperationVisa { get; set; }

    public string? TechnicalRole { get; set; }

    public int? Level { get; set; }

    public string? JobCode { get; set; }

    public int? ElcaTenureMonth { get; set; }

    public bool Active { get; set; } = default!;

    public int? SupervisorId { get; set; }

    public Employee? Supervisor { get; set; }

    public int? RoleId { get; set; }

    public Role? Role { get; set; }

    public string? TimeZoneId { get; set; }

    public string? IdentityNumber { get; set; }

    public string? Phone { get; set; }

    public int? BankAccountId { get; set; }

    public BankAccount? BankAccount { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
  }
}
