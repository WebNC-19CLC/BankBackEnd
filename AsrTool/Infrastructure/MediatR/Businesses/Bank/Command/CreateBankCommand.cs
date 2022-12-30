using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Bank.Command
{
  public class CreateBankCommand : IRequest<BankDto>
  {
    public CreateBankDto Request { get; set; }
  }
}
