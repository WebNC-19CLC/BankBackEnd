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
  public class GetMyNotificationsQueryHandler : IRequestHandler<GetMyNotificationsQuery, ICollection<NotifationDto>>
  {

    private readonly IUserResolver _userResolver;
    private readonly IMapper _mapper;
    private readonly IAsrContext _context;

    public GetMyNotificationsQueryHandler(IUserResolver userResolver, IMapper mapper, IAsrContext context)
    {
      _userResolver = userResolver;
      _mapper = mapper;
      _context = context;
    }

    public async Task<ICollection<NotifationDto>> Handle(GetMyNotificationsQuery request, CancellationToken cancellationToken)
    {
      var user = await _context.Get<Employee>().Include(x => x.BankAccount).ThenInclude(x => x.Notifications).SingleOrDefaultAsync(x => x.Id == _userResolver.CurrentUser.Id);

      if (user.BankAccount == null)
      {
        throw new BusinessException("Bank account not found");
      }

      return _mapper.Map<ICollection<Notification>, ICollection<NotifationDto>>(user.BankAccount.Notifications.OrderByDescending(x => x.Id).ToList());
    }
  }
}
