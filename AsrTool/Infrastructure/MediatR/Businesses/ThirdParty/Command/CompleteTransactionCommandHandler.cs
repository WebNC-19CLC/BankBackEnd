using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.ThirdParty.Command
{
    public class CompleteTransactionCommandHandler : IRequestHandler<CompleteTransactionCommand,TransactionDto>
    {
        private readonly IMediator _mediator;
        private readonly IAsrContext _context;
        private readonly IMapper _mapper;

        public CompleteTransactionCommandHandler(IMediator mediator, IAsrContext context, IMapper mapper)
        {
            _mediator = mediator;
            _context = context;
            _mapper = mapper;
        }

        public async Task<TransactionDto> Handle(CompleteTransactionCommand request, CancellationToken cancellationToken)
        {
            Transaction transaction = await _context.Get<Transaction>().SingleOrDefaultAsync(x => x.Id == request.Id);
            
            if(transaction == null)
            {
                throw new NotFoundException<Transaction>(request.Id);
            }

            BankAccount toAccount = await _context.Get<BankAccount>().SingleOrDefaultAsync(x => x.AccountNumber == transaction.ToAccountNumber);

            if(toAccount == null)
            {
                throw new NotFoundException<BankAccount>(transaction.ToAccountNumber);
            }

            toAccount.Balance = toAccount.Balance + transaction.Amount;
            await _context.UpdateAsync(toAccount);

            //TODO: change transaction type to enum completed
            transaction.Type = "Completed";
            await _context.UpdateAsync(transaction);

            return _mapper.Map<Transaction, TransactionDto>(transaction);
        }
    }
}
