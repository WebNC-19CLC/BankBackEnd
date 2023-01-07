using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Admin.Command
{
  public class UpdateEmployeeCommand : IRequest<EmployeeDto>
  {
    public EmployeeDto Request { get; set; }
  }
}
