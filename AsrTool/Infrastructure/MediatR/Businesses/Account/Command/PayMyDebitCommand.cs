using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class PayMyDebitCommand : IRequest
  {
    public int Id { get; set; }
  }
}
