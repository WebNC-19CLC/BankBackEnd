using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
  { 
    private readonly IAsrContext _context;

    public ForgotPasswordCommandHandler(IAsrContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
      var account = await _context.Get<Employee>().SingleOrDefaultAsync(x => x.Username == request.Request.Username);

      if (account == null) {
        throw new BusinessException("Account not found");
      }

      account.Password = BC.HashPassword(request.Request.Password);

      await _context.UpdateAsync(account);
      await _context.SaveChangesAsync();

      return Unit.Value;
    }
  }
}
