using AsrTool.Infrastructure.Domain.Entities.Interfaces;
using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.Infrastructure.Domain.Entities
{
  public class OTP : IIdentity, IVersioning, IAuditing
  {
    public int Id { get; set; }

    public byte[]? RowVersion { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = default!;

    public DateTime ModifiedOn { get; set; }

    public string ModifiedBy { get; set; } = default!;

    public string Code { get; set; }

    public string? Type { get; set; } = default!;

    public DateTime ExpiredAt { get; set; }

    public OTPStatus Status { get; set; } = OTPStatus.NotUsed;
  }
}
