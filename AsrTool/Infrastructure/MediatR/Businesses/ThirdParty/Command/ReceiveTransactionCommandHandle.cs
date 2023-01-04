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
            var to = await _context.Get<BankAccount>().SingleOrDefaultAsync(x => x.AccountNumber == request.makeTransaction.ToAccountNumber);
            if(to == null)
            {
                throw new NotFoundException<BankAccount>(request.makeTransaction.ToAccountNumber);
            }

            var transaction = new Transaction
            {
                BankSourceId = request.bankSourceId,
                FromId = null,
                FromAccountNumber = request.makeTransaction.FromAccountNumber,
                BankDestinationId = null,
                ToAccountNumber = to.AccountNumber,
                ToId = to.Id,
                Amount = request.makeTransaction.Amount,
                Type = request.makeTransaction.Type,
                Description = request.makeTransaction.Description != null && request.makeTransaction.Description != string.Empty ? request.makeTransaction.Description : $"Account {request.makeTransaction.FromAccountNumber} transfer {request.makeTransaction.Amount} units",
                ChargeReceiver = false,
                TransactionFee = Constants.Fee.TransactionFee,
            };

            await _context.AddRangeAsync(transaction);
            await _context.SaveChangesAsync();

            return _mapper.Map<Transaction, TransactionDto>(transaction);
        }
    }
}
