using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Domain.Entities;

namespace AsrTool.Infrastructure.Context.Seeders.Imp
{
  public class RoleSeeder : BaseSeeder<Role>
  {
    private readonly IAsrContext _context;
    protected override IEnumerable<Role> SeedItems()
    {
      var roles = new[]
      {
        Constants.Roles.TECHNICAL_USER,
        Constants.Roles.Admin,
        Constants.Roles.User,
        Constants.Roles.Employee,
      };

      foreach (var role in roles)
      {
        yield return new Role { Name = role, Rights = SharedRoles.GetRights(role) };
      }
    }

    protected override async Task AddToContext(ICollection<Role> items)
    {
      await _context.AddRangeAsync(items);
    }

    public RoleSeeder(IStore store, IAsrContext context) : base(store)
    {
      _context = context;
    }
  }
}