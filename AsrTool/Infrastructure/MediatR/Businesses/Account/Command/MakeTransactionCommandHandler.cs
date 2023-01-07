using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Common.ThirdParty;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class MakeTransactionCommandHandler : IRequestHandler<MakeTransactionCommand>
  {
    private readonly IAsrContext _asrContext;
    private readonly IUserResolver _userResolver;
    private readonly IMediator _mediator;

    public MakeTransactionCommandHandler(IAsrContext asrContext, IUserResolver userResolver, IMediator mediator)
    {
      _asrContext = asrContext;
      _userResolver = userResolver;
      _mediator = mediator;
    }

    public async Task<Unit> Handle(MakeTransactionCommand request, CancellationToken cancellationToken)
    {
      var from = await _asrContext.Get<Domain.Entities.BankAccount>().Include(x => x.OTP).SingleOrDefaultAsync(x => x.AccountNumber == request.MakeTransactionDto.FromAccountNumber);

      if (from == null && request.MakeTransactionDto.Type != "Charge")
      {
        throw new NotFoundException();
      }
      if (request.MakeTransactionDto.Type == "Charge")
      {
        var to = await _asrContext.Get<Domain.Entities.BankAccount>().SingleOrDefaultAsync(x => x.AccountNumber == request.MakeTransactionDto.ToAccountNumber);
        to.Balance = to.Balance + request.MakeTransactionDto.Amount;

        await _asrContext.UpdateAsync(to);

        var trans = new Transaction
        {
          FromId = from?.Id,
          FromAccountNumber = from?.AccountNumber,
          ToAccountNumber = to.AccountNumber,
          ToId = to.Id,
          Amount = request.MakeTransactionDto.Amount,
          Type = request.MakeTransactionDto.Type,
          Description = request.MakeTransactionDto.Description != null && request.MakeTransactionDto.Description != string.Empty ? request.MakeTransactionDto.Description : $"Account {_userResolver.CurrentUser.FullName} transfer {request.MakeTransactionDto.Amount} units",
          ChargeReceiver = request.MakeTransactionDto.ChargeReceiver,
          TransactionFee = Constants.Fee.TransactionFee,
        };

        await _asrContext.AddRangeAsync(trans);

        await _asrContext.SaveChangesAsync();

        trans = await _asrContext.Get<Transaction>().Include(x => x.To).ThenInclude(x => x.User).Include(x => x.From).ThenInclude(x => x.User)
          .SingleOrDefaultAsync(x => x.Id == trans.Id);

        if (trans.Type == "Charge")
        {
          await _mediator.Send(new MakeNotificationCommand()
          {
            Request = new MakeNotificationDto
            {
              AccountId = trans.To.Id,
              Description = $"You have been recharged money with the amount of money for {trans.Amount}",
              Type = "Transaction",
            }
          });
        }
      }
      else if (request.MakeTransactionDto.BankId == null)
      {
        var to = await _asrContext.Get<Domain.Entities.BankAccount>().SingleOrDefaultAsync(x => x.AccountNumber == request.MakeTransactionDto.ToAccountNumber);

        if (request.MakeTransactionDto.Amount > from?.Balance)
        {
          throw new Exception("Not enough balance");
        }

        if (request.MakeTransactionDto.ChargeReceiver)
        {
          from.Balance = from.Balance - request.MakeTransactionDto.Amount;
          to.Balance = to.Balance + request.MakeTransactionDto.Amount - Constants.Fee.TransactionFee;

        }
        else
        {
          from.Balance = from.Balance - request.MakeTransactionDto.Amount - Constants.Fee.TransactionFee;
          to.Balance = to.Balance + request.MakeTransactionDto.Amount;
        }

        from.OTP.Status = Domain.Enums.OTPStatus.Used;
        await _asrContext.UpdateAsync(from);

        await _asrContext.UpdateAsync(to);

        var trans = new Transaction
        {
          FromId = from.Id,
          FromAccountNumber = from.AccountNumber,
          ToAccountNumber = to.AccountNumber,
          ToId = to.Id,
          Amount = request.MakeTransactionDto.Amount,
          Type = request.MakeTransactionDto.Type,
          Description = request.MakeTransactionDto.Description != null && request.MakeTransactionDto.Description != string.Empty ? request.MakeTransactionDto.Description : $"Account {_userResolver.CurrentUser.FullName} transfer {request.MakeTransactionDto.Amount} units",
          ChargeReceiver = request.MakeTransactionDto.ChargeReceiver,
          TransactionFee = Constants.Fee.TransactionFee,
        };

        await _asrContext.AddRangeAsync(trans);

        await _asrContext.SaveChangesAsync();

        trans = await _asrContext.Get<Transaction>().Include(x => x.To).ThenInclude(x => x.User).Include(x => x.From).ThenInclude(x => x.User)
        .SingleOrDefaultAsync(x => x.Id == trans.Id);

        if (trans.Type == "Transaction")
        {
          await _mediator.Send(new MakeNotificationCommand()
          {
            Request = new MakeNotificationDto
            {
              AccountId = trans.To.Id,
              Description = $"User {from.User.FullName} have sent you the amount of money for {trans.Amount} with description: {trans.Amount}",
              Type = trans.Type,
            }
          });
        }
      }
      else if (request.MakeTransactionDto.BankId != null)
      {
        var bank = await _asrContext.Get<Domain.Entities.Bank>().SingleOrDefaultAsync(x => x.Id == request.MakeTransactionDto.BankId);

        if (bank == null)
        {
          throw new NotFoundException<Domain.Entities.Bank>(request.MakeTransactionDto.BankId.Value);
        }

        IThirdPartyRequestHandler RequestHandler = ThirdPartyRequestHandlerFactory.GetThirdPartyRequestHandler(bank);

        TransactionDto assosiatedBankTransaction = await RequestHandler.CommandMakeTransaction(request.MakeTransactionDto);

        await RequestHandler.CommandCompleteTransaction(assosiatedBankTransaction.Id.ToString());

        from.Balance = from.Balance - request.MakeTransactionDto.Amount - Constants.Fee.TransactionFee;

        from.OTP.Status = Domain.Enums.OTPStatus.Used;

        await _asrContext.UpdateAsync(from);

        var trans = new Transaction
        {
          FromId = from.Id,
          FromAccountNumber = from.AccountNumber,
          ToAccountNumber = request.MakeTransactionDto.ToAccountNumber,
          BankSourceId = bank.Id,
          ToId = null,
          Amount = request.MakeTransactionDto.Amount,
          Type = request.MakeTransactionDto.Type,
          Description = request.MakeTransactionDto.Description != null && request.MakeTransactionDto.Description != string.Empty ? request.MakeTransactionDto.Description : $"Account {_userResolver.CurrentUser.FullName} transfer {request.MakeTransactionDto.Amount} units",
          ChargeReceiver = false,
          TransactionFee = Constants.Fee.TransactionFee,
        };

        await _asrContext.AddRangeAsync(trans);

        await _asrContext.SaveChangesAsync();
      }
      return Unit.Value;

    }

  }
}
