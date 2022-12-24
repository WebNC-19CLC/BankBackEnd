using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.MediatR.Businesses.User.Queries;
using AsrTool.UnitTest._Common.Extensions;
using AsrTool.UnitTest._Common.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AsrTool.UnitTest.UserRequests.Queries
{
  public class SearchUsersByTermQueryHandlerTest : BaseHandlerTest<SearchUsersByTermQuery, IEnumerable<UserRefDto>>
  {
    [Fact]
    public async Task TestHandler_WhenSearchTermOnly_ThenReturnUsersWithNameOrVisaContainSearchTerm()
    {
      // Arrange
      await CreateTestData();

      var query = new SearchUsersByTermQuery()
      {
        Request = new SearchUsersByTermFilterDto()
        {
          SearchTerm = "Anh"
        }
      };

      // Act
      var result = (await Service.Handle(query, new System.Threading.CancellationToken())).ToArray();

      // Assert
      Assert.Equal(3, result.Length);
      Assert.Equal("Anh", result[0].Visa);
      Assert.Equal("Ahg", result[1].Visa);
      Assert.Equal("Abc", result[2].Visa);
    }

    [Fact]
    public async Task TestHandler_WhenSearchTermAndExceptRoles_ThenReturnUsersWithNameOrVisaContainSearchTermWithoutThoseRoles()
    {
      // Arrange
      var (dbRoles, _) = await CreateTestData();

      var query = new SearchUsersByTermQuery()
      {
        Request = new SearchUsersByTermFilterDto()
        {
          SearchTerm = "Anh",
          ExcludedRoleIds = new int[1] { dbRoles[1].Id }
        }
      };

      // Act
      var result = (await Service.Handle(query, new System.Threading.CancellationToken())).ToArray();

      // Assert
      Assert.Equal(2, result.Length);
      Assert.Equal("Ahg", result[0].Visa);
      Assert.Equal("Abc", result[1].Visa);
    }

    [Fact]
    public async Task TestHandler_WhenSearchTermAndExceptUsers_ThenReturnUsersWithNameOrVisaContainSearchTermExceptThoseInputUsers()
    {
      // Arrange
      var (_, dbUsers) = await CreateTestData();

      var query = new SearchUsersByTermQuery()
      {
        Request = new SearchUsersByTermFilterDto()
        {
          SearchTerm = "Anh",
          ExcludedIds = new int[1] { dbUsers[0].Id }
        }
      };

      // Act
      var result = (await Service.Handle(query, new System.Threading.CancellationToken())).ToArray();

      // Assert
      Assert.Equal(2, result.Length);
      Assert.Equal("Anh", result[0].Visa);
      Assert.Equal("Ahg", result[1].Visa);
    }

    [Fact]
    public async Task TestHandler_WhenSearchTermAndExceptRolesAndUsers_ThenReturnUsersWithNameOrVisaContainSearchTermWithoutThoseRolesAndExceptThoseInputUsers()
    {
      // Arrange
      var (dbRoles, dbUsers) = await CreateTestData();

      var query = new SearchUsersByTermQuery()
      {
        Request = new SearchUsersByTermFilterDto()
        {
          SearchTerm = "Anh",
          ExcludedRoleIds = new int[1] { dbRoles[1].Id },
          ExcludedIds = new int[1] { dbUsers[0].Id }
        }
      };

      // Act
      var result = (await Service.Handle(query, new System.Threading.CancellationToken())).ToArray();

      // Assert
      Assert.Single(result);
      Assert.Equal("Ahg", result[0].Visa);
    }

    private async Task<(Infrastructure.Domain.Entities.Role[] roles, Infrastructure.Domain.Entities.Employee[] employees)> CreateTestData()
    {
      var role1 = DataHelper.RoleData.CreateRole("role1");
      var role2 = DataHelper.RoleData.CreateRole("role2");
      await Container.AddEntitiesAsync(role1, role2);

      var user1 = DataHelper.EmployeeData.CreateEmployee("Abc", role1, "Anh", "Nguyen");
      var user2 = DataHelper.EmployeeData.CreateEmployee("Ahg", role1, "Anh", "Hoang Giang");
      var user3 = DataHelper.EmployeeData.CreateEmployee("Anh", role2, "An", "Huynh");
      var user4 = DataHelper.EmployeeData.CreateEmployee("Ghi", role2, "Giang", "Ho");
      var user5 = DataHelper.EmployeeData.CreateEmployee("Jkl", role2, "John", "Lang De");
      await Container.AddEntitiesAsync(user1, user2, user3, user4, user5);
      await Container.SaveChangesAsync();

      var dbRoles = Container.GetEntities<Infrastructure.Domain.Entities.Role>().OrderBy(r => r.Name).ToArray();
      var dbUsers = Container.GetEntities<Infrastructure.Domain.Entities.Employee>().OrderBy(r => r.Visa).ToArray();

      return (dbRoles, dbUsers);
    }
  }
}
