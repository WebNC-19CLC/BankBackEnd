namespace AsrTool.Infrastructure.Exceptions
{
  public class ArgumentException<T> : ArgumentException
  {
    public ArgumentException(string message = null, Exception exception = null) : base($"Entity='{typeof(T).Name}', Error='{message}'", exception)
    {
    }
  }
}