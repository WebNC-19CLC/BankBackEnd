using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.ThirdParty.Queries
{
    public class GetCustomerInfoQueryHandler : IRequestHandler<GetCustomerInfoQuery, ShortAccountDto>
    {
        private readonly IMediator _mediator;
        private readonly IAsrContext _context;
        private readonly IMapper _mapper;

        public GetCustomerInfoQueryHandler(IMediator mediator, IAsrContext context,IMapper mapper)
        {
            _mediator = mediator;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ShortAccountDto> Handle(GetCustomerInfoQuery request, CancellationToken cancellationToken)
        {
            var account = await _context.Get<BankAccount>().Include(x => x.User).SingleOrDefaultAsync(x => x.AccountNumber == request.AccountNumber);
            if(account == null)
            {
                throw new NotFoundException<BankAccount>(request.AccountNumber);
            }

            var result = new ShortAccountDto
            {
                AccountNumber = account.AccountNumber,
                FullName = account.User.FullName,
            };
            return result;
        }
    }
}
