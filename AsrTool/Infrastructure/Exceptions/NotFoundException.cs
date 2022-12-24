using AsrTool.Infrastructure.Domain.Entities.Interfaces;

namespace AsrTool.Infrastructure.Exceptions
{
  public class NotFoundException : BusinessException
  {
    public NotFoundException(string message = null, Exception exception = null) : base(message, exception)
    {
    }
  }

  public class NotFoundException<T> : NotFoundException
  {
    public NotFoundException(IIdentity identifyEntity, Exception exception = null) : base($"Entity='{typeof(T).Name}', Id='{identifyEntity?.Id}' is not found", exception)
    {
    }

    public NotFoundException(int id, Exception exception = null) : base($"Entity='{typeof(T).Name}', Id='{id}' is not found", exception)
    {
    }

    public NotFoundException(string identity, Exception exception = null) : base($"Entity='{typeof(T).Name}', Id='{identity}' is not found", exception)
    {
    }
  }

}