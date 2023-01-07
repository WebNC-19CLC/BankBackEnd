using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Admin.Command
{
  public class DeleteEmployeeCommnadHandler : IRequestHandler<DeleteEmployeeCommnad>
  {
    private readonly IAsrContext _asrContext;

    public DeleteEmployeeCommnadHandler(IAsrContext asrContext)
    {
      _asrContext = asrContext;
    }

    public async Task<Unit> Handle(DeleteEmployeeCommnad request, CancellationToken cancellationToken)
    {
      var employee = await _asrContext.Get<Employee>().SingleOrDefaultAsync(x => x.Id == request.Id);

      if (employee == null)
      {
        throw new BusinessException("Employee not found");
      }

      await _asrContext.RemoveAsync(employee);
      await _asrContext.SaveChangesAsync();

      return Unit.Value;
    }
  }
}
