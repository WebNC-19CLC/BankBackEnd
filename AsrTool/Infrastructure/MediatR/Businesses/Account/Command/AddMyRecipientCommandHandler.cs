using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Common.ThirdParty;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
    public class AddMyRecipientCommandHandler : IRequestHandler<AddMyRecipientCommand, RecipientDto>
    {
        private readonly IUserResolver _userResolver;
        private readonly IMapper _mapper;
        private readonly IAsrContext _context;

        public AddMyRecipientCommandHandler(IUserResolver userResolver, IMapper mapper, IAsrContext context)
        {
            _userResolver = userResolver;
            _mapper = mapper;
            _context = context;
        }

        public async Task<RecipientDto> Handle(AddMyRecipientCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Get<Employee>().SingleOrDefaultAsync(x => x.Id == _userResolver.CurrentUser.Id);
            var bankAccount = await _context.Get<BankAccount>().Include(x => x.Recipients).SingleOrDefaultAsync(x => x.Id == user.BankAccountId);

            if (bankAccount.Recipients.Any(x => x.AccountNumber == request.Request.AccountNumber))
            {
                throw new BusinessException("This account number is exist in your recipients list");
            }

            string defaultSuggestedName;

            if (request.Request.BankDestinationId == null)
            {
                var bankAccountRecp = await _context.Get<Employee>().Include(x => x.BankAccount).SingleOrDefaultAsync(x => x.BankAccount.AccountNumber == request.Request.AccountNumber);

                if (bankAccountRecp == null)
                {
                    throw new BusinessException("This account number is not existed");
                }
                defaultSuggestedName = bankAccountRecp.FullName;
            } else
            {
                var bank = await _context.Get<Domain.Entities.Bank>().SingleOrDefaultAsync(x => x.Id == request.Request.BankDestinationId);

                if(bank == null)
                {
                    throw new BusinessException("Assoaciated bank is not existed");
                }

                IThirdPartyRequestHandler RequestHandler = ThirdPartyRequestHandlerFactory.GetThirdPartyRequestHandler(bank);

                var bankAccountDto = await RequestHandler.QueryInfo(request.Request.AccountNumber);

                defaultSuggestedName = bankAccountDto.FullName;
            }

            var recipient = new Recipient
            {
                AccountNumber = request.Request.AccountNumber,
                SuggestedName = request.Request.SuggestedName != null && request.Request.SuggestedName != String.Empty ? request.Request.SuggestedName : defaultSuggestedName,
                BankDestinationId = request.Request.BankDestinationId,
            };

            bankAccount.Recipients.Add(recipient);

            await _context.AddAsync(recipient);
            await _context.UpdateAsync(bankAccount);
            await _context.SaveChangesAsync();

            return _mapper.Map<Recipient, RecipientDto>(recipient);
        }
    }
}
