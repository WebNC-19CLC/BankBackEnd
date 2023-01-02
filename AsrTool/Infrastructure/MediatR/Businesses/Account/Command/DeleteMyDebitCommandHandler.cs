using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class DeleteMyDebitCommandHandler : IRequestHandler<DeleteMyDebitCommand>
  {
    private readonly IUserResolver _userResolver;
    private readonly IMapper _mapper;
    private readonly IAsrContext _context;
    private readonly IMediator _mediator;

    public DeleteMyDebitCommandHandler(IUserResolver userResolver, IMapper mapper, IAsrContext context, IMediator mediator)
    {
      _userResolver = userResolver;
      _mapper = mapper;
      _context = context;
      _mediator = mediator;
    }

    public async Task<Unit> Handle(DeleteMyDebitCommand request, CancellationToken cancellationToken)
    {
      var debit = await _context.Get<Debit>().SingleOrDefaultAsync(x => x.Id == request.Id);

      var user = await _context.Get<Employee>().Include(x => x.BankAccount).ThenInclude(x => x.Debits).SingleOrDefaultAsync(x => x.Id == _userResolver.CurrentUser.Id);

      if (!user.BankAccount.Debits.Any(x => x.Id == request.Id))
      {
        throw new BusinessException("Cannot find this debit in your list");
      }

      if (debit == null)
      {
        throw new BusinessException("Recipient not exist");
      }
      int sendNotId;
      bool userInDebt = false;
      if (debit.FromAccountNumber == user.BankAccount.AccountNumber)
      {
        sendNotId = (int)debit.ToId;
        userInDebt = true;
      }
      else {
        sendNotId = (int)debit.FromId;
      }

      await _context.RemoveAsync(debit);
      await _context.SaveChangesAsync();

      string mes = userInDebt?  "you in debt with" : "you make with";

      await _mediator.Send(new MakeNotificationCommand() { Request = new MakeNotificationDto { Description = $"{user.FullName} has deleted debit that {mes} amount of {debit.Amount}", AccountId = sendNotId } });

      return Unit.Value;
    }
  }
}
