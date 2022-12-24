using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.MediatR.Businesses.User.Commands;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AsrTool.UnitTest.UserRequests.Commands
{
  public class AssignUsersToRoleCommandValidatorTest : BaseValidatorTest<AssignUsersToRoleCommand>
  {
    [Theory]
    [InlineData(1, null, new int[] { 1 }, true)]
    [InlineData(null, true, new int[] { 1 }, true)]
    [InlineData(null, null, new int[] { 1 }, false)]
    [InlineData(null, false, new int[] { 1 }, false)]
    public async Task TestValidator(int? roleId, bool? removeCurrentRole, int[] userIds, bool expectedValidateResult)
    {
      // Arrange
      var command = new AssignUsersToRoleCommand()
      {
        Request = new AssignUsersToRoleRequestDto()
        {
          RoleId = roleId,
          RemoveCurrentRole = removeCurrentRole,
          UserIds = userIds
        }
      };

      // Act
      var failures = await ValidateAsync(command);

      // Assert
      Assert.Equal(expectedValidateResult, !failures.Any());
    }
  }
}
