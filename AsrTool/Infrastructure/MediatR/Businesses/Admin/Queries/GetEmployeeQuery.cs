using MediatR;
using AsrTool.Dtos;

namespace AsrTool.Infrastructure.MediatR.Businesses.Admin.Queries
{
  public class GetEmployeeQuery : IRequest<ICollection<EmployeeDto>>
  {
  }
}
