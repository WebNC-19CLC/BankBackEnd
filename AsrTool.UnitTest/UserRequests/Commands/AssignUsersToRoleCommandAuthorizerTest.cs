using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.MediatR.Businesses.User.Commands;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AsrTool.UnitTest.UserRequests.Commands
{
  public class AssignUsersToRoleCommandAuthorizerTest : BaseAuthorizerTest<AssignUsersToRoleCommand>
  {
    [Theory]
    [InlineData(new Right[] { Right.WriteRole }, true)]
    [InlineData(new Right[] { Right.WriteRoleAll }, true)]
    [InlineData(new Right[] { }, false)]
    public async Task TestAuthorizer(Right[] rights, bool expectedAuthResult)
    {
      // Arrange
      SetupRights(rights);

      var command = new AssignUsersToRoleCommand()
      {
        Request = new AssignUsersToRoleRequestDto()
      };

      // Act
      var result = await AuthorizeAsync(command);

      // Assert
      Assert.True(result.All(r => r.Succeeded == expectedAuthResult));
    }
  }
}
