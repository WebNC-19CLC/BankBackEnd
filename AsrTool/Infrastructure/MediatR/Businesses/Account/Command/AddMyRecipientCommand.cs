using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class AddMyRecipientCommand : IRequest<RecipientDto>
  {
    public CreateRecipientDto Request { get; set; }
  }
}
