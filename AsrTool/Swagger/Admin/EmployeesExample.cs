using Swashbuckle.AspNetCore.Filters;
using AsrTool.Dtos;

namespace AsrTool.Swagger.Admin
{
  public class EmployeesExample : IMultipleExamplesProvider<List<EmployeeDto>>
  {
    public IEnumerable<SwaggerExample<List<EmployeeDto>>> GetExamples()
    {
      yield return SwaggerExample.Create(
         "Example",
         new List<EmployeeDto>() {new EmployeeDto()
        {
         Id = 1,
         Address = "HCM",
         Email = "employee@gmail.com",
         FirstName = "Em",
         LastName = "Ployee",
         IndentityNumber = "2432513533",
         IsActive = true,
         Phone = "0341231111",
         Role = "Employee",
         Username = "employee"
         } }
       );
    }
  }
}
