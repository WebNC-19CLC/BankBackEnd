﻿using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class MakeTransactionCommandHandler : IRequestHandler<MakeTransactionCommand>
  {
    private readonly IAsrContext _asrContext;
    private readonly IUserResolver _userResolver;

    public MakeTransactionCommandHandler(IAsrContext asrContext, IUserResolver userResolver)
    {
      _asrContext = asrContext;
      _userResolver = userResolver;
    }

    public async Task<Unit> Handle(MakeTransactionCommand request, CancellationToken cancellationToken)
    {
      var from  =await _asrContext.Get<Domain.Entities.BankAccount>().SingleOrDefaultAsync(x => x.AccountNumber == request.MakeTransactionDto.From);

      if (from == null) {
        throw new NotFoundException();
      }
  
      var to  = await _asrContext.Get<Domain.Entities.BankAccount>().SingleOrDefaultAsync(x => x.AccountNumber == request.MakeTransactionDto.To);
     
      if (to == null)
      {
        throw new NotFoundException();
      }

      if (request.MakeTransactionDto.Amount > from.Balance) {
        throw new Exception("Not enough balance");
      }

      if (request.MakeTransactionDto.ChargeReceiver)
      {
        from.Balance = from.Balance - request.MakeTransactionDto.Amount;
        to.Balance = from.Balance + request.MakeTransactionDto.Amount - Constants.Fee.TransactionFee;

      }
      else {
        from.Balance = from.Balance - request.MakeTransactionDto.Amount - Constants.Fee.TransactionFee;
        to.Balance = from.Balance + request.MakeTransactionDto.Amount ;

      }
     
      await _asrContext.UpdateAsync(from);
      await _asrContext.UpdateAsync(to);

      var trans = new Transaction
      {
        FromId = from.Id,
        ToId = to.Id,
        Amount = request.MakeTransactionDto.Amount,
        Type = "Transaction",
        Description = $"Account {_userResolver.CurrentUser.FullName} transfer {request.MakeTransactionDto.Amount} units",
        ChargeReceiver = request.MakeTransactionDto.ChargeReceiver,
        TransactionFee = Constants.Fee.TransactionFee
      };

      await _asrContext.AddRangeAsync(trans);

      await _asrContext.SaveChangesAsync();

      return Unit.Value;
    }
  }
}
