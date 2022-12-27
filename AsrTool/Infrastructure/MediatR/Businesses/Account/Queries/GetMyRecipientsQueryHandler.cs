using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Queries
{
  public class GetMyRecipientsQueryHandler : IRequestHandler<GetMyRecipientsQuery, ICollection<RecipientDto>>
  {
    private readonly IUserResolver _userResolver;
    private readonly IMapper _mapper;
    private readonly IAsrContext _context;

    public GetMyRecipientsQueryHandler(IUserResolver userResolver, IMapper mapper, IAsrContext context)
    {
      _userResolver = userResolver;
      _mapper = mapper;
      _context = context;
    }

    public async Task<ICollection<RecipientDto>> Handle(GetMyRecipientsQuery request, CancellationToken cancellationToken)
    {
      var user = await _context.Get<Employee>().Include(x => x.BankAccount).ThenInclude(x => x.Recipients).SingleOrDefaultAsync(x => x.Id == _userResolver.CurrentUser.Id);
      
      if (user.BankAccount == null) {
        throw new NotFoundException();
      }

      return _mapper.Map<ICollection<Recipient>, ICollection<RecipientDto>>(user.BankAccount.Recipients);
    }
  }
}
