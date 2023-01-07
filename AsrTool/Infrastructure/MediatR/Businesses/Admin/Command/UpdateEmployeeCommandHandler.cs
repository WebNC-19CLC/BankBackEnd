using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Admin.Command
{
  public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
  {
    private readonly IAsrContext _asrContext;

    public UpdateEmployeeCommandHandler(IAsrContext asrContext)
    {
      _asrContext = asrContext;
    }

    public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
      var employee = await _asrContext.Get<Employee>().Include(x => x.Role).SingleOrDefaultAsync(x => x.Id == request.Request.Id);

      if (employee == null) {
        throw new BusinessException("Employee not found");
      }

      employee.FirstName = request.Request.FirstName;
      employee.LastName = request.Request.LastName;
      employee.IdentityNumber = request.Request.IndentityNumber;
      employee.Phone = request.Request.Phone;
      employee.Email = request.Request.Email;
      employee.Active = request.Request.IsActive;
      employee.Site = request.Request.Address;

      await _asrContext.UpdateAsync(employee);
      await _asrContext.SaveChangesAsync();

      return new EmployeeDto
      {
        Id = employee.Id,
        Address = employee.Site,
        IsActive = employee.Active,
        Email = employee.Email,
        FirstName = employee.FullName,
        LastName = employee.LastName,
        IndentityNumber = employee.IdentityNumber,
        Phone = employee.Phone,
        Role = employee.Role.Name
      };
    }
  }
}
