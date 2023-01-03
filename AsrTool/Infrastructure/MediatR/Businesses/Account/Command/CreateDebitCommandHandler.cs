using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NLog.LayoutRenderers.Wrappers;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class CreateDebitCommandHandler : IRequestHandler<CreateDebitCommand, DebitDto>
  {
    private readonly IUserResolver _userResolver;
    private readonly IMapper _mapper;
    private readonly IAsrContext _context;
    private readonly IMediator _mediator;

    public CreateDebitCommandHandler(IUserResolver userResolver, IMapper mapper, IAsrContext context, IMediator mediator)
    {
      _userResolver = userResolver;
      _mapper = mapper;
      _context = context;
      _mediator = mediator;
    }

    public async Task<DebitDto> Handle(CreateDebitCommand request, CancellationToken cancellationToken)
    {
      var user = await _context.Get<Employee>().SingleOrDefaultAsync(x => x.Id == _userResolver.CurrentUser.Id);
      var bankAccount = await _context.Get<BankAccount>().Include(x => x.Recipients).SingleOrDefaultAsync(x => x.Id == user.BankAccountId);

      if (bankAccount == null)
      {
        throw new BusinessException("This account does not have bank account.");
      }

      var targetAccount = await _context.Get<BankAccount>().Include(x => x.User).SingleOrDefaultAsync(x => x.AccountNumber == request.Request.AccountNumber);

      int? fromId;
      int? toId;
      string? fromAccountNumber;
      string? toAccountNumber;
      string Debter;

      if (request.Request.SelfInDebt)
      {
        fromId = bankAccount.Id;
        fromAccountNumber = bankAccount.AccountNumber;
        Debter = user.FullName;
        toId = targetAccount.Id;
        toAccountNumber = targetAccount.AccountNumber;
      }
      else
      {
        fromId = targetAccount.Id;
        fromAccountNumber = targetAccount.AccountNumber;
        Debter = targetAccount.User.FullName;
        toId = bankAccount.Id;
        toAccountNumber = bankAccount.AccountNumber;
      }

      var debit = new Debit
      {
        FromId = fromId,
        FromAccountNumber = fromAccountNumber,
        ToId = toId,
        ToAccountNumber = toAccountNumber,
        Description = request.Request.Description,
        BankDestinationId = request.Request.BankDestinationId,
        Amount = request.Request.Amount,
        DateDue = request.Request.DateDue
      };

      await _context.AddRangeAsync(debit);
      await _context.SaveChangesAsync();

      var resultToReturn = await _context.Get<Debit>().Include(x => x.From).ThenInclude(x => x.User)
        .Include(x => x.To).ThenInclude(x => x.User)
        .SingleOrDefaultAsync(x => x.Id == debit.Id);

      if (request.Request.SelfInDebt)
        await _mediator.Send(new MakeNotificationCommand() { Request = new MakeNotificationDto { Description = $"{Debter} are in debt of you for {debit.Amount}", AccountId = (int)toId } });
      else
        await _mediator.Send(new MakeNotificationCommand() { Request = new MakeNotificationDto { Description = $"You are in debt of {Debter} for {debit.Amount}", AccountId = (int)fromId } });

      return new DebitDto
      {
        FromAccountNumber = resultToReturn.FromAccountNumber,
        ToAccountNumber = resultToReturn.ToAccountNumber,
        FromUser = resultToReturn?.From?.User?.FullName,
        ToUser = resultToReturn?.To?.User?.FullName,
        BankDestinationId = resultToReturn?.BankDestinationId,
        BankSourceId = resultToReturn?.BankSourceId,
        Amount = resultToReturn.Amount,
        DateDue = resultToReturn.DateDue,
        IsPaid = resultToReturn.IsPaid,
        Id = resultToReturn.Id,
        Time = resultToReturn.CreatedOn
      };
    }
  }
}
