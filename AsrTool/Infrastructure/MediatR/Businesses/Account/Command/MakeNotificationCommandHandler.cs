using AsrTool.Dtos;
using AsrTool.Hubs;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class MakeNotificationCommandHandler : IRequestHandler<MakeNotificationCommand>
  {
    private readonly IAsrContext _asrContext;
    private readonly IUserResolver _userResolver;
    private IHubContext<MessageHub, IMessageHub> _messageHub;
    private readonly IMapper _mapper;

    public MakeNotificationCommandHandler(IAsrContext asrContext, IUserResolver userResolver, IHubContext<MessageHub, IMessageHub> messageHub, IMapper mapper)
    {
      _asrContext = asrContext;
      _userResolver = userResolver;
      _messageHub = messageHub;
      _mapper = mapper;
    }

    public async Task<Unit> Handle(MakeNotificationCommand request, CancellationToken cancellationToken)
    {
        var noti = new Notification
        {
          Description = request.Request.Description,
          BankAccountId = request.Request.AccountId,
          Type = request.Request.Type,
        };

        var user = await _asrContext.Get<Employee>().SingleOrDefaultAsync(x => x.BankAccountId == request.Request.AccountId);

        await _asrContext.AddRangeAsync(noti);
        await _asrContext.SaveChangesAsync();

        await _messageHub.Clients.User(user.Username).SendNotificationToUser(_mapper.Map<Notification, NotifationDto>(noti));

      return Unit.Value;

    }
  }
}
