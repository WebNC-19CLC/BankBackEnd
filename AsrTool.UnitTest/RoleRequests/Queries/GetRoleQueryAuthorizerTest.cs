using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.MediatR.Businesses.Role.Queries;
using AsrTool.UnitTest._Common.Extensions;
using AsrTool.UnitTest._Common.Helpers;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Xunit;

namespace AsrTool.UnitTest.RoleRequests.Queries
{
  public class GetRoleQueryAuthorizerTest : BaseAuthorizerTest<GetRoleQuery>
  {
    [Theory]
    [InlineData(true, new Right[0], false)]
    [InlineData(true, new Right[] { Right.ReadRole }, true)]
    [InlineData(true, new Right[] { Right.ReadRole, Right.ReadRoleAll }, true)]
    [InlineData(false, new Right[0], false)]
    [InlineData(false, new Right[] { Right.ReadRole }, false)]
    [InlineData(false, new Right[] { Right.ReadRole, Right.ReadRoleAll }, true)]
    public async Task TestAuthorizer(bool createdByTestUser, Right[] rights, bool testResult)
    {
      // Arrange
      var creatingRole = DataHelper.RoleData.CreateRole();
      await Container.AddEntitiesAsync(creatingRole);
      await (createdByTestUser ? Container.SaveChangesAsync() : Container.SaveChangesAsync("another user"));

      SetupRights(rights);
      var dbRole = Container.GetEntities<Infrastructure.Domain.Entities.Role>().Single(x => x.Name == creatingRole.Name);
      var request = new GetRoleQuery() { RoleId = dbRole.Id };

      // Act & Assert
      if (!testResult)
      {
        await Assert.ThrowsAsync<SecurityException>(async () => await AuthorizeAsync(request));
        return;
      }

      // Act
      var result = await AuthorizeAsync(request);

      // Assert
      Assert.True(result.All(x => x.Succeeded));
    }
  }
}