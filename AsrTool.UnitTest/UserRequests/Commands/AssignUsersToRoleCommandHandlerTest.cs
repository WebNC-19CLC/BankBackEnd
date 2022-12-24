using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.Exceptions;
using AsrTool.Infrastructure.MediatR.Businesses.User.Commands;
using AsrTool.UnitTest._Common.Extensions;
using AsrTool.UnitTest._Common.Helpers;
using MediatR;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AsrTool.UnitTest.UserRequests.Commands
{
  public class AssignUsersToRoleCommandHandlerTest : BaseHandlerTest<AssignUsersToRoleCommand, Unit>
  {
    [Fact]
    public async Task TestHandler_WhenInputRoleIsFound_ThenAssignRoleToUsers()
    {
      // Arrange
      var role1 = DataHelper.RoleData.CreateRole("role1");
      var role2 = DataHelper.RoleData.CreateRole("role2");
      var user1 = DataHelper.EmployeeData.CreateEmployee("user1", role1);
      var user2 = DataHelper.EmployeeData.CreateEmployee("user2", role1);
      await Container.AddEntitiesAsync(role1, role2);
      await Container.AddEntitiesAsync(user1, user2);
      await Container.SaveChangesAsync();

      var role2Id = Container.GetEntities<Infrastructure.Domain.Entities.Role>().Single(r => r.Name == role2.Name).Id;
      var userIds = Container.GetEntities<Infrastructure.Domain.Entities.Employee>().Select(r => r.Id);
      await Container.ClearChangesAsync();

      var command = new AssignUsersToRoleCommand()
      {
        Request = new AssignUsersToRoleRequestDto()
        {
          RemoveCurrentRole = false,
          RoleId = role2Id,
          UserIds = userIds.ToArray()
        }
      };

      // Act
      await Service.Handle(command, new System.Threading.CancellationToken());

      // Assert
      var dbUsers = Container.GetEntities<Infrastructure.Domain.Entities.Employee>();
      Assert.True(dbUsers.All(r => r.RoleId == command.Request.RoleId));
    }

    [Fact]
    public async Task TestHandler_WhenInputRoleIsNotFound_ThenThrowError()
    {
      // Arrange
      var role1 = DataHelper.RoleData.CreateRole("role1");
      var user1 = DataHelper.EmployeeData.CreateEmployee("user1", role1);
      var user2 = DataHelper.EmployeeData.CreateEmployee("user2", role1);
      await Container.AddEntitiesAsync(role1);
      await Container.AddEntitiesAsync(user1, user2);
      await Container.SaveChangesAsync();

      var userIds = Container.GetEntities<Infrastructure.Domain.Entities.Employee>().Select(r => r.Id);
      await Container.ClearChangesAsync();

      var command = new AssignUsersToRoleCommand()
      {
        Request = new AssignUsersToRoleRequestDto()
        {
          RemoveCurrentRole = false,
          RoleId = int.MaxValue,
          UserIds = userIds.ToArray()
        }
      };

      // Act & Assert
      await Assert.ThrowsAsync<NotFoundException<Infrastructure.Domain.Entities.Role>>(
        () => Service.Handle(command, new System.Threading.CancellationToken()));
    }

    [Fact]
    public async Task TestHandler_WhenRemoveCurrentRoleMode_ThenRemoveCurrentRoleOfUsers()
    {
      // Arrange
      var role1 = DataHelper.RoleData.CreateRole("role1");
      var user1 = DataHelper.EmployeeData.CreateEmployee("user1", role1);
      var user2 = DataHelper.EmployeeData.CreateEmployee("user2", role1);
      await Container.AddEntitiesAsync(user1, user2);
      await Container.SaveChangesAsync();

      var userIds = Container.GetEntities<Infrastructure.Domain.Entities.Employee>().Select(r => r.Id);
      await Container.ClearChangesAsync();

      var command = new AssignUsersToRoleCommand()
      {
        Request = new AssignUsersToRoleRequestDto()
        {
          RemoveCurrentRole = true,
          UserIds = userIds.ToArray()
        }
      };

      // Act
      await Service.Handle(command, new System.Threading.CancellationToken());

      // Assert
      var dbUsers = Container.GetEntities<Infrastructure.Domain.Entities.Employee>();
      Assert.True(dbUsers.All(r => r.RoleId == null));
    }
  }
}
