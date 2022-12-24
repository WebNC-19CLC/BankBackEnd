namespace AsrTool.Infrastructure.Context.Seeders
{
  public interface IStore
  {
    Task AddToStore<T>(ICollection<T> items);

    Task<ICollection<T>> GetFromStore<T>();
  }
}