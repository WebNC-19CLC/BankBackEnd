using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class DeleteMyDebitCommand : IRequest
  {
    public int Id { get; set; }
  }
}
