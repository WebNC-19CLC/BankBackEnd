using AsrTool.Dtos;
using AsrTool.Infrastructure.Domain.Entities.Interfaces;
using AsrTool.Infrastructure.Extensions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.Common.Grid
{
  public abstract class GridQueryHandler<TQuery, TEntity, TDto> : IRequestHandler<TQuery, DataSourceResultDto<TDto>>
    where TQuery : GridQuery<TEntity, TDto>
    where TEntity : IIdentity, IVersioning
  {
    protected IMapper Mapper { get; set; }

    protected virtual string[] PreloadColumns => Array.Empty<string>();

    protected GridQueryHandler(IMapper mapper)
    {
      Mapper = mapper;
    }

    public virtual async Task<DataSourceResultDto<TDto>> Handle(TQuery request, CancellationToken cancellationToken)
    {
      var columns = PreloadColumns;
      if (request.DataSourceRequest.PreloadAllData != true)
      {
        columns = typeof(TDto).GetProperties().Select(x => x.Name).ToArray();
      }

      var sortedQuery = request.Query
        .ProjectTo<TDto>(Mapper.ConfigurationProvider, null, columns)
        .ApplySortFields(request.DataSourceRequest.Sorts.ToArray());

      var result = new DataSourceResultDto<TDto>
      {
        Total = await sortedQuery.CountAsync(cancellationToken),

        Data = !request.DataSourceRequest.PreloadAllData.HasValue
        || request.DataSourceRequest.PreloadAllData.Value
        ? await sortedQuery.ToListAsync(cancellationToken)
        : await sortedQuery
          .Skip(request.DataSourceRequest.Skip)
          .Take(request.DataSourceRequest.Take)
          .ToListAsync(cancellationToken)
      };

      return result;
    }
  }
}
