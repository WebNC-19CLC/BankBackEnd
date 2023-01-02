using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class CreateEmployeeAccountCommand : IRequest<BankAccountDto>
  {
    public CreateAccountDto Request { get; set; }
  }
}
