﻿using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Queries
{
  public class GetCurrentAccountQueryHandler : IRequestHandler<GetCurrentAccountQuery, AccountDto>
  {
    private readonly IUserResolver _userResolver;
    private readonly IMapper _mapper;
    private readonly IAsrContext _context;

    public GetCurrentAccountQueryHandler(IUserResolver userResolver, IMapper mapper, IAsrContext context)
    {
      _userResolver = userResolver;
      _mapper = mapper;
      _context = context;
    }

    public async Task<AccountDto> Handle(GetCurrentAccountQuery request, CancellationToken cancellationToken)
    {
      var currentUser = await _context.Get<Employee>().Include(x => x.Role).Include(x => x.BankAccount).ThenInclude(x => x.Recipients).SingleOrDefaultAsync(x => x.Id == _userResolver.CurrentUser.Id);

      if (currentUser == null) {
        throw new NotFoundException<BankAccount>(_userResolver.CurrentUser.Id);
      }

      var bankAccount = currentUser.BankAccount;

      var result = new AccountDto
      {
        FullName = currentUser.FullName,
        AccountNumber = bankAccount?.AccountNumber,
        Balance = bankAccount?.Balance,
        IsActive = currentUser.Active,
        Email = currentUser.Email,
        Role = currentUser.Role.Name,
        BankAccountId = bankAccount?.Id,
        Recipients = bankAccount != null ? _mapper.Map<ICollection<Recipient>, ICollection<RecipientDto>>(bankAccount.Recipients) : new List<RecipientDto>(),
      };

      return result;
    }
  }
}
