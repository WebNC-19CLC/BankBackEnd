using System;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.UnitTest._Common.Helpers
{
  public static class DataHelper
  {
    public struct RoleData
    {
      public static Role CreateRole(string name = "TestingRole", params Right[] rights)
      {
        return new Role { Name = name, Rights = rights };
      }
    }

    public struct EmployeeData
    {
      public static Employee CreateEmployee(
        string visa = "TEST",
        Role? role = null,
        string firstName = "First",
        string lastName = "Last",
        string legalUnit = "ELCA",
        string orgUnit = "SolutionEngineering",
        string site = "Ho Chi Minh City",
        DateTime? entryDate = null
        )
      {
        return new Employee
        {
          Visa = visa,
          Role = role,
          FirstName = firstName,
          LastName = lastName,
          LegalUnit = legalUnit,
          OrganizationUnit = orgUnit,
          Site = site,
          EntryDate = entryDate ?? DateTime.UtcNow,
          Username = visa,
          TimeZoneId = Constants.TimeZoneId.DEFAULT
        };
      }
    }
  }
}