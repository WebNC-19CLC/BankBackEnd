using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class EditMyRecipientCommand : IRequest<RecipientDto>
  {
    public RecipientDto Request { get; set; }
  }
}
