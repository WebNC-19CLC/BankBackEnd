namespace AsrTool.Infrastructure.Context.Seeders.Imp
{
  public class Store : IStore
  {
    private readonly IDictionary<Type, ICollection<object>> _store = new Dictionary<Type, ICollection<object>>();

    public Task AddToStore<T>(ICollection<T> items)
    {
      var key = typeof(T);
      if (!_store.ContainsKey(key))
      {
        _store.Add(key, new List<object>());
      }

      foreach (var item in items)
      {
        _store[key].Add(item);
      }
      return Task.CompletedTask;
    }

    public Task<ICollection<T>> GetFromStore<T>()
    {
      var key = typeof(T);
      if (!_store.ContainsKey(key))
      {
        return Task.FromResult<ICollection<T>>(new List<T>());
      }

      return Task.FromResult<ICollection<T>>(_store[key].Cast<T>().ToList());
    }
  }
}