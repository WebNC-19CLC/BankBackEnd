using AsrTool.Dtos;
using MediatR;

namespace AsrTool.Infrastructure.Common.Grid
{
  public abstract class GridQuery<TEntity, TDto> : IRequest<DataSourceResultDto<TDto>>
  {
    public DataSourceRequestDto DataSourceRequest { get; set; }

    public IQueryable<TEntity> Query { get; set; }
  }
}
