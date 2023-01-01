using AsrTool.Infrastructure.Common;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class SendOTPEmailCommandHandler : IRequestHandler<SendOTPEmailCommand>
  {
    private readonly IEmailService _emailService;

    public SendOTPEmailCommandHandler(IEmailService emailService)
    {
      _emailService = emailService;
    }
    public async Task<Unit> Handle(SendOTPEmailCommand request, CancellationToken cancellationToken)
    {
      await _emailService.EmailAsync(request.OTP, request.Email);

      return Unit.Value;
    }
  }
}
