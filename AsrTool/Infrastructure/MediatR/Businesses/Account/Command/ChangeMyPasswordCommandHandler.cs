using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class ChangeMyPasswordCommandHandler : IRequestHandler<ChangeMyPasswordCommand>
  {
    private readonly IAsrContext _asrContext;
    private readonly IUserResolver _userResolver;
    private readonly IMediator _mediator;

    public ChangeMyPasswordCommandHandler(IAsrContext asrContext, IUserResolver userResolver, IMediator mediator)
    {
      _asrContext = asrContext;
      _userResolver = userResolver;
      _mediator = mediator;
    }

    public async Task<Unit> Handle(ChangeMyPasswordCommand request, CancellationToken cancellationToken)
    {
      var user = await _asrContext.Get<Employee>().SingleOrDefaultAsync(x => x.Id == _userResolver.CurrentUser.Id);

      bool matchPassword = BC.Verify(request.Request.Password, user.Password);

      if (!matchPassword) throw new BusinessException("Wrong password input");

      user.Password = BC.HashPassword(request.Request.NewPassword);

      await _asrContext.UpdateAsync(user);
      await _asrContext.SaveChangesAsync();

      return Unit.Value;
    }
  }
}
