namespace AsrTool.Infrastructure.Exceptions
{
  public class UnauthorizerException : Exception
  {
    public UnauthorizerException(string message = null, Exception exception = null) : base(message, exception)
    {
    }
  }
}
