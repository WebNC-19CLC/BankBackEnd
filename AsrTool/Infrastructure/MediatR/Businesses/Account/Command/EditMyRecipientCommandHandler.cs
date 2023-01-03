using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class EditMyRecipientCommandHandler : IRequestHandler<EditMyRecipientCommand, RecipientDto>
  {
    private readonly IUserResolver _userResolver;
    private readonly IMapper _mapper;
    private readonly IAsrContext _context;

    public EditMyRecipientCommandHandler(IUserResolver userResolver, IMapper mapper, IAsrContext context)
    {
      _userResolver = userResolver;
      _mapper = mapper;
      _context = context;
    }

    public async Task<RecipientDto> Handle(EditMyRecipientCommand request, CancellationToken cancellationToken)
    {
      var recipient = await _context.Get<Recipient>().SingleOrDefaultAsync(x => x.Id == request.Request.Id);

      if (recipient == null) {
        throw new NotFoundException<Recipient>(request.Request.Id);
      }

      recipient.SuggestedName = request.Request.SuggestedName != string.Empty || request.Request.SuggestedName != null ? request.Request.SuggestedName : recipient.SuggestedName;
      recipient.AccountNumber = request.Request.AccountNumber;
      recipient.BankDestinationId = request.Request.BankDestinationId;

      await _context.UpdateAsync(recipient);
      await _context.SaveChangesAsync();

      return _mapper.Map<Recipient, RecipientDto>(recipient);
    }
  }
}
