﻿using AsrTool.Infrastructure.Domain.Entities.Interfaces;

namespace AsrTool.Infrastructure.Domain.Entities
{
  public class Bank : IIdentity, IVersioning, IAuditing
  {
    public int Id { get; set; }

    public byte[]? RowVersion { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = default!;

    public DateTime ModifiedOn { get; set; }

    public string ModifiedBy { get; set; } = default!;

    public string Name { get; set; }

    public string API { get; set; }

    public string EncryptRsaPublicKey { get; set; }

    public string DecryptRsaPrivateKey { get; set; }

    public string DecryptPublicKey { get; set; }

    }
}
