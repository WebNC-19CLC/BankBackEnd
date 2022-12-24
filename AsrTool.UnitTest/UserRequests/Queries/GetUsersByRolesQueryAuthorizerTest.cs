using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.MediatR.Businesses.User.Queries;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AsrTool.UnitTest.UserRequests.Queries
{
  public class GetUsersByRolesQueryAuthorizerTest : BaseAuthorizerTest<GetUsersByRolesQuery>
  {
    [Theory]
    [InlineData(new Right[] { Right.ReadRole }, false)]
    [InlineData(new Right[] { Right.ReadRoleAll }, true)]
    [InlineData(new Right[] { }, false)]
    public async Task TestAuthorizer(Right[] rights, bool expectedAuthResult)
    {
      // Arrange
      SetupRights(rights);

      var command = new GetUsersByRolesQuery();

      // Act
      var result = await AuthorizeAsync(command);

      // Assert
      Assert.True(result.All(r => r.Succeeded == expectedAuthResult));
    }
  }
}
