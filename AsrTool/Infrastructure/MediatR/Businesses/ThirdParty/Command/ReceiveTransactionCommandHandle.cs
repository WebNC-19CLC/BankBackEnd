using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

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
            var to = await _context.Get<BankAccount>().SingleOrDefaultAsync(x => x.AccountNumber == request.transaction.ToAccountNumber);
            if(to == null)
            {
                throw new NotFoundException<BankAccount>(request.transaction.ToAccountNumber);
            }

            to.Balance = to.Balance + request.transaction.Amount;
            await _context.UpdateAsync(to);

            return request.transaction;
        }
    }
}
