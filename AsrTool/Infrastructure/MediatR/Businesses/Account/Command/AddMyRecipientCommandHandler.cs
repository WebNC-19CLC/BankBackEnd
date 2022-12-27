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

      var recipient = new Recipient
      {
        AccountNumber = request.Request.AccountNumber,
        SuggestedName = request.Request.SuggestedName,
        BankDestinationId = request.Request.BankDestinationId,
      };

      bankAccount.Recipients.Add(recipient);

      await _context.AddAsync(recipient);
      bankAccount.Recipients.Add(recipient);
      await _context.UpdateAsync(bankAccount);
      await _context.SaveChangesAsync();

      return _mapper.Map<Recipient, RecipientDto>(recipient);
    }
  }
}
