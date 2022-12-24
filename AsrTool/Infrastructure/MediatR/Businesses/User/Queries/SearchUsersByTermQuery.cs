using AsrTool.Dtos.UserRoleDtos;
using MediatR;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Queries
{
  public class SearchUsersByTermQuery : IRequest<IEnumerable<UserRefDto>>
  {
    public SearchUsersByTermFilterDto Request { get; set; } = new SearchUsersByTermFilterDto();
  }
}
