﻿using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class TransferMoneyCommandHandler : IRequestHandler<TransferMoneyCommand>
  {
    private readonly IUserResolver _userResolver;
    private readonly IMapper _mapper;
    private readonly IAsrContext _context;
    private readonly IMediator _mediator;

    public TransferMoneyCommandHandler(IUserResolver userResolver, IMapper mapper, IAsrContext context, IMediator mediator)
    {
      _userResolver = userResolver;
      _mapper = mapper;
      _context = context;
      _mediator = mediator;
    }
    public async Task<Unit> Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
    {
      var user = await _context.Get<Employee>().Include(x => x.BankAccount).AsNoTracking().SingleOrDefaultAsync(x => x.Id == _userResolver.CurrentUser.Id);

      if (user.BankAccount == null) {
        throw new BusinessException("This user do not have bank account");
      }

      var transaction = new MakeTransactionDto {
        BankId = request.Request.BankId,
        ChargeReceiver = request.Request.ChargeReceiver,
        Amount = request.Request.Amount,
        FromAccountNumber = user.BankAccount.AccountNumber,
        ToAccountNumber = request.Request.ToAccountNumber,
      };

      return await _mediator.Send(new MakeTransactionCommand() {MakeTransactionDto = transaction } );
    }
  }
}
