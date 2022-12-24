using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.MediatR.Businesses.Role.Commands;
using AsrTool.UnitTest._Common.Extensions;
using AsrTool.UnitTest._Common.Helpers;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AsrTool.UnitTest.RoleRequests.Commands
{
  public class ResetRolesCommandHandlerTest : BaseHandlerTest<ResetRolesCommand, Unit>
  {
    [Fact]
    public async Task TestHandler_WhenRoleIdsIsDefined_ThenResetThoseRoles()
    {
      // Arrange
      var roleEmployee = DataHelper.RoleData.CreateRole(Constants.Roles.HR, Right.ReadEmployeeAll);
      await Container.AddEntitiesAsync(roleEmployee);
      await Container.SaveChangesAsync();

      int roleEmployeeId = Container.GetEntities<Infrastructure.Domain.Entities.Role>().Single(r => r.Name == Constants.Roles.HR).Id;
      await Container.ClearChangesAsync();

      var command = new ResetRolesCommand()
      {
        Request = new ResetRolesRequestDto()
        {
          RoleIds = new int[] { roleEmployeeId }
        }
      };

      // Act
      await Service.Handle(command, new System.Threading.CancellationToken());

      // Assert
      var dbRoleEmployee = Container.GetEntities<Infrastructure.Domain.Entities.Role>().Single(r => r.Id == roleEmployeeId);
      Assert.Equal(SharedRoles.GetRights(Constants.Roles.HR), dbRoleEmployee.Rights);
    }

    [Fact]
    public async Task TestHandler_WhenRoleIdsIsEmpty_ThenResetAllRoles()
    {
      // Arrange
      var roleAdmin = DataHelper.RoleData.CreateRole(Constants.Roles.EB, Right.ReadEmployeeAll);
      var roleEmployee = DataHelper.RoleData.CreateRole(Constants.Roles.TECHNICAL_USER, Right.JobRunner);
      await Container.AddEntitiesAsync(roleAdmin, roleEmployee);
      await Container.SaveChangesAsync();
      await Container.ClearChangesAsync();

      var command = new ResetRolesCommand()
      {
        Request = new ResetRolesRequestDto()
        {
          RoleIds = Array.Empty<int>()
        }
      };

      // Act
      await Service.Handle(command, new System.Threading.CancellationToken());

      // Assert
      var dbRoleAdmin = Container.GetEntities<Infrastructure.Domain.Entities.Role>().Single(r => r.Name == Constants.Roles.EB);
      Assert.Equal(SharedRoles.GetRights(Constants.Roles.EB), dbRoleAdmin.Rights);

      var dbRoleEmployee = Container.GetEntities<Infrastructure.Domain.Entities.Role>().Single(r => r.Name == Constants.Roles.TECHNICAL_USER);
      Assert.Equal(SharedRoles.GetRights(Constants.Roles.TECHNICAL_USER), dbRoleEmployee.Rights);
    }
  }
}
