using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using AsrTool.Infrastructure.Auth;

namespace AsrTool.Infrastructure.MediatR.Businesses.Admin.Queries
{
  public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, ICollection<EmployeeDto>>
  {
    private readonly IAsrContext _asrContext;

    public GetEmployeeQueryHandler(IAsrContext asrContext)
    {
      _asrContext = asrContext;
    }

    public async Task<ICollection<EmployeeDto>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
    {
      var employees = _asrContext.Get<Employee>().Include(x => x.Role).Where(x => x.Role.Name == Constants.Roles.Employee).OrderByDescending(x => x.Id);

      return await employees.Select(x => new EmployeeDto
      {
        Username = x.Username,
        Id = x.Id,
        Address = x.Site,
        IsActive = x.Active,
        Email = x.Email,
        FirstName = x.FullName,
        LastName = x.LastName,
        IndentityNumber = x.IdentityNumber,
        Phone = x.Phone,
        Role = x.Role.Name
      }).ToListAsync();
    }
  }
}
