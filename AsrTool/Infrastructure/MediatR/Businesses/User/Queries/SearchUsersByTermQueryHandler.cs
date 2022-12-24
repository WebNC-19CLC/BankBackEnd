using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Context;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Queries
{
  public class SearchUsersByTermQueryHandler : IRequestHandler<SearchUsersByTermQuery, IEnumerable<UserRefDto>>
  {
    private readonly IAsrContext _asrContext;
    private readonly IMapper _mapper;

    public SearchUsersByTermQueryHandler(IAsrContext asrContext, IMapper mapper)
    {
      _asrContext = asrContext;
      _mapper = mapper;
    }

    public async Task<IEnumerable<UserRefDto>> Handle(SearchUsersByTermQuery query, CancellationToken cancellationToken)
    {
      var excludedRoleIds = query.Request.ExcludedRoleIds;
      string searchTerm = query.Request.SearchTerm;
      int[] excludedIds = query.Request.ExcludedIds;

      var dbEmployees = _asrContext.Get<Domain.Entities.Employee>();

      if (excludedRoleIds?.Any() == true)
      {
        dbEmployees = dbEmployees.Where(r => !r.RoleId.HasValue || !excludedRoleIds.Contains(r.RoleId.Value));
      }

      if (excludedIds?.Any() == true)
      {
        dbEmployees = dbEmployees.Where(r => !excludedIds.Contains(r.Id));
      }

      if (!string.IsNullOrWhiteSpace(searchTerm))
      {
        dbEmployees = dbEmployees.Where(r =>
          (r.FirstName != null && r.FirstName.Contains(searchTerm)) ||
          (r.LastName != null && r.LastName.Contains(searchTerm)) ||
          r.Visa.Contains(searchTerm));
      }

      return await dbEmployees
                    .OrderBy(r => r.FirstName).ThenBy(r => r.LastName).ThenBy(r => r.Visa)
                    .Take(Constants.Pagination.DEFAULT_PAGE_SIZE)
                    .ProjectTo<UserRefDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
    }
  }
}
