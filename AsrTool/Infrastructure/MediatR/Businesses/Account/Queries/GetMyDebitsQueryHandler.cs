using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Queries
{
  public class GetMyDebitsQueryHandler : IRequestHandler<GetMyDebitsQuery, ICollection<DebitDto>>
  {
    private readonly IAsrContext _asrContext;
    private readonly IUserResolver _userResolver;

    public GetMyDebitsQueryHandler(IAsrContext asrContext, IUserResolver userResolver)
    {
      _asrContext = asrContext;
      _userResolver = userResolver;
    }

    public async Task<ICollection<DebitDto>> Handle(GetMyDebitsQuery request, CancellationToken cancellationToken)
    {
      var user = await _asrContext.Get<Employee>().Include(x => x.BankAccount).SingleOrDefaultAsync(x => x.Id == _userResolver.CurrentUser.Id);

      if (user.BankAccount == null)
      {
        throw new BusinessException("This user do not have bank account");
      }

      return await _asrContext.Get<Debit>().Include(x => x.From).ThenInclude(x => x.User)
        .Include(x => x.To).ThenInclude(x => x.User)
        .Where(x => x.FromId == user.BankAccount.Id || x.ToId == user.BankAccount.Id)
        .OrderByDescending(x => x.Id)
        .Select(x => new DebitDto
        {
          Id = x.Id,
          FromAccountNumber = x.From.AccountNumber,
          ToAccountNumber = x.To.AccountNumber,
          Amount = x.Amount,
          Time = x.CreatedOn,
          BankDestinationId = x.BankDestinationId,
          BankSourceId = x.BankSourceId,
          DateDue = x.DateDue,
          IsPaid = x.IsPaid,
          FromUser = x.From.User.FullName,
          ToUser = x.To.User.FullName
        })
        .ToListAsync();
    }
  }
}
