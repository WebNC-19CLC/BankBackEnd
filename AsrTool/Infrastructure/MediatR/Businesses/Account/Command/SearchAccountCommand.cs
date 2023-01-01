using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class SearchAccountCommand : IRequest<ShortAccountDto?>
  {
    public string AccountNumber { get; set; }
  }
}
