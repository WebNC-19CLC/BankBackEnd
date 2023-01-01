using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AsrTool.Infrastructure.MediatR.Businesses.ThirdParty.Command
{
    public class ReceiveTransactionCommandHandle : IRequestHandler<ReceiveTransactionCommand,TransactionDto>
    {
        private readonly IMediator _mediator;
        private readonly IAsrContext _context;
        private readonly IMapper _mapper;

        public ReceiveTransactionCommandHandle(IMediator mediator, IAsrContext context, IMapper mapper)
        {
            _mediator = mediator;
            _context = context;
            _mapper = mapper;
        }

        public async Task<TransactionDto> Handle(ReceiveTransactionCommand request, CancellationToken cancellationToken)
        {
            var to = await _context.Get<BankAccount>().SingleOrDefaultAsync(x => x.AccountNumber == request.MakeTransaction.ToAccountNumber);
            if(to == null)
            {
                throw new NotFoundException<BankAccount>(request.MakeTransaction.ToAccountNumber);
            }

            //to.Balance = to.Balance + request.MakeTransaction.Amount;
            //await _context.UpdateAsync(to);

            var transaction = new Transaction
            {
                FromId = null,
                FromAccountNumber = request.MakeTransaction.FromAccountNumber,
                ToAccountNumber = to.AccountNumber,
                ToId = to.Id,
                Amount = request.MakeTransaction.Amount,
                Type = request.MakeTransaction.Type,
                Description = request.MakeTransaction.Description != null && request.MakeTransaction.Description != string.Empty ? request.MakeTransaction.Description : $"Account {request.MakeTransaction.FromAccountNumber} transfer {request.MakeTransaction.Amount} units",
                ChargeReceiver = false,
                TransactionFee = Constants.Fee.TransactionFee,
            };

            await _context.AddRangeAsync(transaction);
            await _context.SaveChangesAsync();

            return _mapper.Map<Transaction, TransactionDto>(transaction);
        }
    }
}
