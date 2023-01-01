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

    public DeleteMyDebitCommandHandler(IUserResolver userResolver, IMapper mapper, IAsrContext context)
    {
      _userResolver = userResolver;
      _mapper = mapper;
      _context = context;
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

      await _context.RemoveAsync(debit);
      await _context.SaveChangesAsync();
      return Unit.Value;
    }
  }
}
