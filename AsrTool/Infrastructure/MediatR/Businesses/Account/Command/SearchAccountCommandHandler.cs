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
    public class SearchAccountCommandHandler : IRequestHandler<SearchAccountCommand, ShortAccountDto>
    {
        private readonly IAsrContext _asrContext;
        private readonly IUserResolver _userResolver;
        private readonly IMediator _mediator;

        public SearchAccountCommandHandler(IAsrContext asrContext, IUserResolver userResolver, IMediator mediator)
        {
            _asrContext = asrContext;
            _userResolver = userResolver;
            _mediator = mediator;
        }

        public async Task<ShortAccountDto?> Handle(SearchAccountCommand request, CancellationToken cancellationToken)
        {
            var bankAccount = await _asrContext.Get<BankAccount>().SingleOrDefaultAsync(x => x.UserId == _userResolver.CurrentUser.Id);

            if (request.bankId == null)
            {
                var searchUser = await _asrContext.Get<Employee>().Include(x => x.BankAccount).SingleOrDefaultAsync(x => x.BankAccount.AccountNumber == request.AccountNumber && x.BankAccount.AccountNumber != bankAccount.AccountNumber);
                if (searchUser == null)
                {
                    return null;
                }
                else
                {
                    return new ShortAccountDto
                    {
                        BankAccountId = searchUser.BankAccount.Id,
                        AccountNumber = searchUser.BankAccount.AccountNumber,
                        FullName = searchUser.FullName
                    };
                }
            }
            else
            {
                var bank = await _asrContext.Get<Domain.Entities.Bank>().SingleOrDefaultAsync(x => x.Id == request.bankId);

                if (bank == null)
                {
                    throw new BusinessException("Assoaciated bank is not existed");
                }

                IThirdPartyRequestHandler RequestHandler = ThirdPartyRequestHandlerFactory.GetThirdPartyRequestHandler(bank);

                var searchUser = await RequestHandler.QueryInfo(request.AccountNumber);

                return new ShortAccountDto
                {
                    BankAccountId = null,
                    AccountNumber = searchUser.AccountNumber,
                    FullName = searchUser.FullName
                };
            }

        }
    }
}
