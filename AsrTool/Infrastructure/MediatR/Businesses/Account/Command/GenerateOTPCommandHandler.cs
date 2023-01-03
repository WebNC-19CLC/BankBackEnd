using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class GenerateOTPCommandHandler : IRequestHandler<GenerateOTPCommand>
  {

    private readonly IAsrContext _asrContext;
    private readonly IMediator _mediator;
    private readonly IUserResolver _userResolver;

    public GenerateOTPCommandHandler(IAsrContext asrContext, IUserResolver userResolver, IMediator mediator)
    {
      _asrContext = asrContext;
      _userResolver = userResolver;
      _mediator = mediator;
    }

    public async Task<Unit> Handle(GenerateOTPCommand request, CancellationToken cancellationToken)
    {
      Employee? user;

      if (request.Username == null)
      {
        user = await _asrContext.Get<Employee>().Include(x => x.BankAccount).ThenInclude(x => x.OTP)
          .FirstOrDefaultAsync(x => x.Id == _userResolver.CurrentUser.Id);
      }
      else {
        user = await _asrContext.Get<Employee>().Include(x => x.BankAccount).ThenInclude(x => x.OTP)
          .FirstOrDefaultAsync(x => x.Username == request.Username);
      }

      if (user?.BankAccount == null) {
        throw new BusinessException("This account doesnt have bank account");
      }

      if (user.BankAccount.OTP == null)
      {
        user.BankAccount.OTP = new OTP
        {
          Code = GetRandomNumber(),
          ExpiredAt = DateTime.UtcNow + TimeSpan.FromMinutes(15),
          Status = Domain.Enums.OTPStatus.NotUsed
        };
      }
      else {
        user.BankAccount.OTP.Code = GetRandomNumber();
        user.BankAccount.OTP.ExpiredAt = DateTime.UtcNow + TimeSpan.FromMinutes(15);
        user.BankAccount.OTP.Status = Domain.Enums.OTPStatus.NotUsed;
      }

      await _asrContext.UpdateAsync(user);
      await _asrContext.SaveChangesAsync();

      await _mediator.Send(new SendOTPEmailCommand() {Email = user.Email , OTP = user.BankAccount.OTP.Code });

      return Unit.Value;
    }

    private string GetRandomNumber()
    {
      Random generator = new Random();
      String r = generator.Next(0, 1000000).ToString("D4");
      return r;
    }
  }
}
