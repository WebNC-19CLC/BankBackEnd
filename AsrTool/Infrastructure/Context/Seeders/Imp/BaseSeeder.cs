namespace AsrTool.Infrastructure.Context.Seeders.Imp
{
  public abstract class BaseSeeder<T> : ISeeder
  {
    private static readonly List<Type> SortedPriority = new List<Type>
    {
      // Admin data
      typeof(RoleSeeder),

      // Working Data
      typeof(EmployeeSeeder),
    };

    protected IStore Store { get; }

    protected BaseSeeder(IStore store)
    {
      Store = store;
    }

    public int Priority => SortedPriority.IndexOf(GetType());


    public async Task Seed()
    {
      var seededItems = SeedItems().ToArray();
      await Store.AddToStore(seededItems);
      await AddToContext(seededItems);
    }

    protected abstract IEnumerable<T> SeedItems();
    protected abstract Task AddToContext(ICollection<T> items);
  }
}