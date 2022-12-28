﻿namespace AsrTool.Dtos
{
  public class MakeTransactionDto
  {
    public string FromAccountNumber { get; set; }
    public string ToAccountNumber { get; set; }
    public double Amount { get; set; }
    public int? BankId { get; set; }
    public bool ChargeReceiver { get; set; } = false;
  }
}
