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
  public class PayMyDebitCommandHandler : IRequestHandler<PayMyDebitCommand>
  {
    private readonly IAsrContext _asrContext;
    private readonly IUserResolver _userResolver;
    private readonly IMediator _mediator;

    public PayMyDebitCommandHandler(IAsrContext asrContext, IUserResolver userResolver, IMediator mediator)
    {
      _asrContext = asrContext;
      _userResolver = userResolver;
      _mediator = mediator;
    }
    public async Task<Unit> Handle(PayMyDebitCommand request, CancellationToken cancellationToken)
    {
      var debit = await _asrContext.Get<Debit>().SingleOrDefaultAsync(x => x.Id == request.Id);

      var user = await _asrContext.Get<Employee>().Include(x => x.BankAccount).ThenInclude(x => x.OTP).SingleOrDefaultAsync(x => x.Id == _userResolver.CurrentUser.Id);

      var targetAccouunt = await _asrContext.Get<BankAccount>().Include(x => x.User).SingleOrDefaultAsync(x => x.Id == debit.ToId);

      if (debit.FromAccountNumber != user?.BankAccount?.AccountNumber) {
        throw new UnauthorizerException("Access Denied");
      }

      if (debit.IsPaid)
      {
        throw new BusinessException("Debit is already paid");
      }

      if (!(user.BankAccount.OTP.Code == request.OTP))
      {
        throw new BusinessException("OTP is not match");
      }

      if (user.BankAccount.OTP.Status == Domain.Enums.OTPStatus.Used)
      {
        throw new BusinessException("OTP is used");
      }

      if (!(DateTime.Compare(user.BankAccount.OTP.ExpiredAt, DateTime.UtcNow) > 0))
      {
        throw new BusinessException("Expired OTP");
      }

      var transaction = new MakeTransactionDto
      {
        Description = debit.Description,
        Amount = debit.Amount,
        FromAccountNumber = debit.FromAccountNumber,
        ToAccountNumber = debit.ToAccountNumber,
        BankId = null,
        ChargeReceiver = false,
        Type = "Debit"
      };

      await _mediator.Send(new MakeTransactionCommand() { MakeTransactionDto = transaction });

      await _mediator.Send(new MakeNotificationCommand() { Request = new MakeNotificationDto {Description = $"{user.FullName} pay you {transaction.Amount} of debit.", AccountId = targetAccouunt.Id } });

      debit.IsPaid = true;

      await _asrContext.UpdateAsync(debit);
      await _asrContext.SaveChangesAsync();

      return Unit.Value;
    }
  }
}
