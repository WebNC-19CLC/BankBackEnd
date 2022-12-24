using AsrTool.Dtos;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.MediatR.Businesses.Role.Queries;
using AsrTool.UnitTest._Common.Extensions;
using AsrTool.UnitTest._Common.Helpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AsrTool.UnitTest.RoleRequests.Queries
{
  public class GetRoleQueryHandlerTest : BaseHandlerTest<GetRoleQuery, RoleDto>
  {
    [Fact]
    public async Task TestHandler_WhenRoleWithGivenIdExists_ThenReturnRoleRecord()
    {
      // Arrange
      SetupRights(new[] { Right.ReadRole, Right.ReadRoleAll });
      var createdRole = DataHelper.RoleData.CreateRole();
      await Container.AddEntitiesAsync(createdRole);
      await Container.SaveChangesAsync();

      var dbRole = Container.GetEntities<Role>().Single(x => x.Name == createdRole.Name);
      var request = new GetRoleQuery() { RoleId = dbRole.Id };

      // Act
      var result = await Service.Handle(request, new CancellationToken());

      // Assert
      Assert.Equal(dbRole.Id, result.Id);
      Assert.Equal(dbRole.Name, result.Name);
      Assert.True(dbRole.Rights.All(x => result.Rights.Contains(x)));
      Assert.Equal(dbRole.RowVersion, result.RowVersion);
    }
  }
}