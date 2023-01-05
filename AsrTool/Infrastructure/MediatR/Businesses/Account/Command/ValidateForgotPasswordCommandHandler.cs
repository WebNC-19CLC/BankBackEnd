using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class ValidateForgotPasswordCommandHandler : IRequestHandler<ValidateForgotPasswordCommand>
  {
    private readonly IUserResolver _userResolver;
    private readonly IMapper _mapper;
    private readonly IAsrContext _context;
    private readonly IMediator _mediator;

    public ValidateForgotPasswordCommandHandler(IUserResolver userResolver, IMapper mapper, IAsrContext context, IMediator mediator)
    {
      _userResolver = userResolver;
      _mapper = mapper;
      _context = context;
      _mediator = mediator;
    }

    public async Task<Unit> Handle(ValidateForgotPasswordCommand request, CancellationToken cancellationToken)
    {
      var account = await _context.Get<Employee>().Include(x => x.BankAccount).ThenInclude(x => x.OTP).SingleOrDefaultAsync(x => x.Username == request.Request.Username);

      if (account == null || account.BankAccount == null)
      {
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


      await _context.UpdateAsync(account);
      await _context.SaveChangesAsync();

      return Unit.Value;
    }
  }
}
