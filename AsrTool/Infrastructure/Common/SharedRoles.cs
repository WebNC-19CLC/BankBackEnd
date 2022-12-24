using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.Infrastructure.Common
{
  public static class SharedRoles
  {
    private static IDictionary<string, ICollection<Right>> Map { get; } =
      new Dictionary<string, ICollection<Right>>
      {
        [Constants.Roles.Admin.ToUpperInvariant()] = GetAdminRights(),
        [Constants.Roles.Employee.ToUpperInvariant()] = GetEmployeeRights(),
        [Constants.Roles.User.ToUpperInvariant()] = GetUserRights(),
        [Constants.Roles.TECHNICAL_USER.ToUpperInvariant()] = GetTechnicalUserRights()
      };

    public static ICollection<Right> GetRights(string roleName)
    {
      var roleNameNormalized = $"{roleName}".ToUpperInvariant();
      if (Map.TryGetValue(roleNameNormalized, out var rights))
      {
        return rights;
      }

      throw new NotSupportedException($"RoleName={roleName} is not supported");
    }

    private static ICollection<Right> GetEmployeeRights()
    {
      return new List<Right>
      {
        Right.ReadEmployeeAll,
        Right.WriteEmployeeAll,
        Right.ReadEmployeeInBL,
        Right.WriteEmployeeInBL,

        Right.JobRunner,

        Right.ReadRole,
        Right.ReadRoleAll,
        Right.WriteRole,
        Right.WriteRoleAll,
        Right.ResetRights,
      };
    }

    private static ICollection<Right> GetAdminRights()
    {
      return new List<Right>
      {
        Right.ReadEmployeeAll,
        Right.WriteEmployeeAll,
        Right.ReadEmployeeInBL,
        Right.WriteEmployeeInBL,

        Right.JobRunner,

        Right.ReadRole,
        Right.ReadRoleAll,
        Right.WriteRole,
        Right.WriteRoleAll,
        Right.ResetRights,
      };
    }

    private static ICollection<Right> GetUserRights()
    {
      return new List<Right>
      {
        Right.ReadEmployeeAll,
        Right.WriteEmployeeAll
      };
    }

    private static ICollection<Right> GetTechnicalUserRights()
    {
      return new List<Right>
      {
        Right.JobRunner,

        Right.ReadRole,
        Right.ReadRoleAll
      };
    }
  }
}