using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.MediatR.Businesses.User.Queries;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AsrTool.UnitTest.UserRequests.Queries
{
  public class SearchUsersByTermQueryAuthorizerTest : BaseAuthorizerTest<SearchUsersByTermQuery>
  {
    [Theory]
    [InlineData(new Right[] { }, true)]
    public async Task TestAuthorizer(Right[] rights, bool expectedAuthResult)
    {
      // Arrange
      SetupRights(rights);

      var command = new SearchUsersByTermQuery();

      // Act
      var result = await AuthorizeAsync(command);

      // Assert
      Assert.True(result.All(r => r.Succeeded == expectedAuthResult));
    }
  }
}
