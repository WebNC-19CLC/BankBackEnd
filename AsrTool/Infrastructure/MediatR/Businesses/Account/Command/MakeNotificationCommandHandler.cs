using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class MakeNotificationCommandHandler : IRequestHandler<MakeNotificationCommand>
  {
    private readonly IAsrContext _asrContext;
    private readonly IUserResolver _userResolver;

    public MakeNotificationCommandHandler(IAsrContext asrContext, IUserResolver userResolver)
    {
      _asrContext = asrContext;
      _userResolver = userResolver;
    }

    public async Task<Unit> Handle(MakeNotificationCommand request, CancellationToken cancellationToken)
    {
      var noti = new Notification {
        Description = request.Request.Description,
        BankAccountId = request.Request.AccountId,
      };

      await _asrContext.AddRangeAsync(noti);
      await _asrContext.SaveChangesAsync();

      return Unit.Value;
    }
  }
}
