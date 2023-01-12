using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Admin.Command
{
  public class DeleteEmployeeCommnad : IRequest
  {
    public int Id { get; set; }
  }
}
