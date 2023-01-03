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
      var account = await _context.Get<Employee>().Include(x => x.BankAccount).ThenInclude(x => x.OTP).SingleOrDefaultAsync(x => x.Username == request.Request.Username);

      if (account == null || account.BankAccount == null) {
        throw new BusinessException("Account not found");
      }

      if (!(account.BankAccount.OTP.Code == request.Request.OTP))
      {
        throw new BusinessException("OTP is not match");
      }

      if (account.BankAccount.OTP.Status == Domain.Enums.OTPStatus.Used)
      {
        throw new BusinessException("OTP is used");
      }

      if (!(DateTime.Compare(account.BankAccount.OTP.ExpiredAt, DateTime.UtcNow) > 0))
      {
        throw new BusinessException("Expired OTP");
      }

      account.BankAccount.OTP.Status = Domain.Enums.OTPStatus.Used;

      account.Password = BC.HashPassword(request.Request.Password);

      await _context.UpdateAsync(account);
      await _context.SaveChangesAsync();

      return Unit.Value;
    }
  }
}
