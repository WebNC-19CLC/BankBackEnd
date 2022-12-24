using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class MakeTransactionCommandHandler : IRequestHandler<MakeTransactionCommand>
  {
    private readonly IAsrContext _asrContext;

    public MakeTransactionCommandHandler(IAsrContext asrContext)
    {
      _asrContext = asrContext;
    }

    public async Task<Unit> Handle(MakeTransactionCommand request, CancellationToken cancellationToken)
    {
      var from  =await _asrContext.Get<Domain.Entities.Account>().SingleOrDefaultAsync(x => x.AccountNumber == request.MakeTransactionDto.From);

      if (from == null) {
        throw new NotFoundException();
      }

      var to  = await _asrContext.Get<Domain.Entities.Account>().SingleOrDefaultAsync(x => x.AccountNumber == request.MakeTransactionDto.To);
     
      if (to == null)
      {
        throw new NotFoundException();
      }

      if (request.MakeTransactionDto.Amount > from.Balance) {
        throw new Exception("Not valid balance");
      }

      from.Balance = from.Balance - request.MakeTransactionDto.Amount;
      to.Balance = from.Balance + request.MakeTransactionDto.Amount;
      await _asrContext.UpdateAsync(from);
      await _asrContext.UpdateAsync(to);

      var trans = new Transaction
      {
        FromId = from.Id,
        ToId = to.Id,
        Amount = request.MakeTransactionDto.Amount,
      };

      await _asrContext.AddRangeAsync(trans);

      await _asrContext.SaveChangesAsync();

      return Unit.Value;
    }
  }
}
