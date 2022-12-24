namespace AsrTool.Infrastructure.Context.Seeders
{
  public interface ISeeder
  {
    int Priority { get; }

    Task Seed();
  }
}