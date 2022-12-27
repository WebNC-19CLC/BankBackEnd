using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Queries
{
  public class GetRecipientQueryHandler : IRequestHandler<GetRecipientQuery, RecipientDto>
  {
    private readonly IAsrContext _asrContext;

    public GetRecipientQueryHandler(IAsrContext asrContext)
    {
      _asrContext = asrContext;
    }
    public async Task<RecipientDto> Handle(GetRecipientQuery request, CancellationToken cancellationToken)
    {
      RecipientDto result = null;

      if (request.Request.BankId == null) {
        var temp = await _asrContext.Get<Employee>().Include(x => x.BankAccount).SingleOrDefaultAsync(x => x.BankAccount.AccountNumber == request.Request.AccountNumber);
        result = new RecipientDto
        {
          Id = 0,
          BankDestinationId = null,
          AccountNumber = temp.BankAccount.AccountNumber,
          SuggestedName = temp.FullName
        };
      }

      return result;
    }
  }
}
