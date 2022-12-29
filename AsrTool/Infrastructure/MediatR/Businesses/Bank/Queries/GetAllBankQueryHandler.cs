using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Bank.Queries
{
    public class GetAllQueryHandler : IRequestHandler<GetAllBankQuery, ICollection<BankDto>>
    {
        private readonly IMapper _mapper;
        private readonly IAsrContext _context;

        public GetAllQueryHandler(IAsrContext asrContext, IMapper mapper)
        {
            _context = asrContext;
            _mapper = mapper;
        }
        public async Task<ICollection<BankDto>> Handle(GetAllBankQuery request, CancellationToken cancellationToken)
        {
            var banks = await _context.Get<Domain.Entities.Bank>().Where(x => x.Name != null).ToListAsync();
            return _mapper.Map<ICollection<Domain.Entities.Bank>, ICollection<BankDto>>(banks);
        }
    }
}
