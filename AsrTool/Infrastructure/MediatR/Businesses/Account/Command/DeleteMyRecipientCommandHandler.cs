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
  public class DeleteMyRecipientCommandHandler : IRequestHandler<DeleteMyRecipientCommand>
  {
    private readonly IUserResolver _userResolver;
    private readonly IMapper _mapper;
    private readonly IAsrContext _context;

    public DeleteMyRecipientCommandHandler(IUserResolver userResolver, IMapper mapper, IAsrContext context)
    {
      _userResolver = userResolver;
      _mapper = mapper;
      _context = context;
    }

    public async Task<Unit> Handle(DeleteMyRecipientCommand request, CancellationToken cancellationToken)
    {
      var recipient = await _context.Get<Recipient>().SingleOrDefaultAsync(x => x.Id == request.Id);

      var user = await _context.Get<Employee>().Include(x => x.BankAccount).ThenInclude(x => x.Recipients).SingleOrDefaultAsync(x => x.Id == _userResolver.CurrentUser.Id);
      
      if (!user.BankAccount.Recipients.Any(x => x.Id == request.Id)) {
        throw new BusinessException("Cannot find this recipient in your list");
      }

      if(recipient == null)
      {
        throw new BusinessException("Recipient not exist");
      }

      await _context.RemoveAsync(recipient);
      await _context.SaveChangesAsync();
      return Unit.Value;
    }
  }
}
