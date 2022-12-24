namespace AsrTool.Infrastructure.Exceptions
{
  public class BusinessException : Exception
  {
    public BusinessException(string message = null, Exception exception = null) : base(message, exception)
    {
    }
  }
}