using AsrTool.Dtos;
using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.MediatR.Businesses.User.Queries;
using AsrTool.UnitTest._Common.Extensions;
using AsrTool.UnitTest._Common.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AsrTool.UnitTest.UserRequests.Queries
{
  public class GetUsersByRolesQueryHandlerTest : BaseHandlerTest<GetUsersByRolesQuery, DataSourceResultDto<UserByRoleDto>>
  {
    [Fact]
    public async Task TestHandler_WhenRoleIdsIsDefined_ThenReturnUsersWithThoseRoles()
    {
      // Arrange
      SetupRights(Right.ReadRole);
      await CreateTestData();

      int roleEmployeeId = Container.GetEntities<Infrastructure.Domain.Entities.Role>().Single(r => r.Name == Constants.Roles.EB).Id;

      var query = new GetUsersByRolesQuery()
      {
        RoleIds = new int[] { roleEmployeeId },
        DataSourceRequest = new DataSourceRequestDto()
      };

      // Act
      var result = await Service.Handle(query, new System.Threading.CancellationToken());

      // Assert
      Assert.Equal(2, result.Total);
      Assert.True(result.Data.All(r => r.RoleId == roleEmployeeId));
    }

    [Fact]
    public async Task TestHandler_WhenRoleIdsIsEmpty_ThenReturnAllUsers()
    {
      // Arrange
      SetupRights(Right.ReadRole);
      await CreateTestData();

      var query = new GetUsersByRolesQuery()
      {
        RoleIds = Array.Empty<int>(),
        DataSourceRequest = new DataSourceRequestDto()
      };

      // Act
      var result = await Service.Handle(query, new System.Threading.CancellationToken());

      // Assert
      Assert.Equal(3, result.Total);

      int roleHrId = Container.GetEntities<Infrastructure.Domain.Entities.Role>().Single(r => r.Name == Constants.Roles.HR).Id;
      int roleEbId = Container.GetEntities<Infrastructure.Domain.Entities.Role>().Single(r => r.Name == Constants.Roles.EB).Id;
      Assert.Equal(1, result.Data.Count(r => r.RoleId == roleHrId));
      Assert.Equal(2, result.Data.Count(r => r.RoleId == roleEbId));
    }

    private async Task CreateTestData()
    {
      var roleHr = DataHelper.RoleData.CreateRole(Constants.Roles.HR);
      var roleEb = DataHelper.RoleData.CreateRole(Constants.Roles.EB);
      await Container.AddEntitiesAsync(roleHr, roleEb);

      var user1 = DataHelper.EmployeeData.CreateEmployee("USER1", roleHr);
      var user2 = DataHelper.EmployeeData.CreateEmployee("USER2", roleEb);
      var user3 = DataHelper.EmployeeData.CreateEmployee("USER3", roleEb);
      await Container.AddEntitiesAsync(user1, user2, user3);
      await Container.SaveChangesAsync();
    }
  }
}
