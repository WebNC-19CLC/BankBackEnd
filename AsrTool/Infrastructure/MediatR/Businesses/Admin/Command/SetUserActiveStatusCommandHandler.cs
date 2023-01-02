using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Admin.Command
{
  public class SetUserActiveStatusCommandHandler : IRequestHandler<SetUserActiveStatusCommand>
  {
    private readonly IAsrContext _asrContext;

    public SetUserActiveStatusCommandHandler(IAsrContext asrContext)
    {
      _asrContext = asrContext;
    }

    public async Task<Unit> Handle(SetUserActiveStatusCommand request, CancellationToken cancellationToken)
    {
      var user = await _asrContext.Get<Employee>().Include(x => x.BankAccount).SingleOrDefaultAsync(x => x.BankAccountId == request.Request.BankAccountId);

      if (user == null) {
        throw new BusinessException("Not found this bank account");
      }

      user.Active = request.Request.IsActive;

      await _asrContext.UpdateAsync(user);
      await _asrContext.SaveChangesAsync();
      return Unit.Value;
    }
  }
}
