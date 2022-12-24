namespace AsrTool.Infrastructure.Jobs
{
  public interface IRecurringJob
  {
    Task Enqueue();

    Task Recurring();

    Task Execute(string jobName);
  }
}