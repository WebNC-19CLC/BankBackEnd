using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class ChargeMoneyToAccountCommandHandler : IRequestHandler<ChargeMoneyToAccountCommand>
  {
    private readonly IAsrContext _asrContext;
    private readonly IUserResolver _userResolver;
    private readonly IMediator _mediator;

    public ChargeMoneyToAccountCommandHandler(IAsrContext asrContext, IUserResolver userResolver, IMediator mediator)
    {
      _asrContext = asrContext;
      _userResolver = userResolver;
      _mediator = mediator;
    }

    public async Task<Unit> Handle(ChargeMoneyToAccountCommand request, CancellationToken cancellationToken)
    {
      var bankAccount = await _asrContext.Get<BankAccount>().AsNoTracking().SingleOrDefaultAsync(x => x.AccountNumber == request.Request.AccountNumber);

      if (bankAccount == null) {
        throw new BusinessException("Cannot find this bank account");
      }

      var transaction = new MakeTransactionDto {
        FromAccountNumber = null,
        ToAccountNumber = bankAccount.AccountNumber,
        BankId = null,
        Amount = request.Request.Amount,
        ChargeReceiver = true,
        Type = "Charge",
        Description = string.Empty,
      };

      await _mediator.Send(new MakeTransactionCommand() { MakeTransactionDto = transaction });

      return Unit.Value;
    }
  }
}
