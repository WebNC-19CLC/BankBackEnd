using AsrTool.Infrastructure.MediatR.Businesses.Role.Queries;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AsrTool.UnitTest.RoleRequests.Queries
{
  public class GetRoleQueryValidatorTest : BaseValidatorTest<GetRoleQuery>
  {
    [Theory]
    [InlineData(-1, false)]
    [InlineData(0, false)]
    [InlineData(1, true)]
    public async Task TestValidator(int id, bool testResult)
    {
      // Arrange
      var request = new GetRoleQuery() { RoleId = id };

      // Act
      var result = await ValidateAsync(request);

      // Assert
      Assert.Equal(testResult, !result.Any());
    }
  }
}