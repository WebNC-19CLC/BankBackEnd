using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.MediatR.Businesses.Role.Commands;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AsrTool.UnitTest.RoleRequests.Commands
{
  public class ResetRolesCommandAuthorizerTest : BaseAuthorizerTest<ResetRolesCommand>
  {
    [Theory]
    [InlineData(new Right[] { Right.ResetRights }, true)]
    [InlineData(new Right[] { Right.ReadEmployeeAll }, false)]
    [InlineData(new Right[] { }, false)]
    public async Task TestAuthorizer(Right[] rights, bool expectedAuthResult)
    {
      // Arrange
      SetupRights(rights);

      var command = new ResetRolesCommand()
      {
        Request = new ResetRolesRequestDto()
      };

      // Act
      var result = await AuthorizeAsync(command);

      // Assert
      Assert.True(result.All(r => r.Succeeded == expectedAuthResult));
    }
  }
}
