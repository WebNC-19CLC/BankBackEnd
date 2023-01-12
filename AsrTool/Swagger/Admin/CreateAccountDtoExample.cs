using Swashbuckle.AspNetCore.Filters;
using AsrTool.Dtos;

namespace AsrTool.Swagger.Admin
{
  public class CreateAccountDtoExample : IMultipleExamplesProvider<CreateAccountDto>
  {
    public IEnumerable<SwaggerExample<CreateAccountDto>> GetExamples()
    {
      yield return SwaggerExample.Create(
        "Example",
        new CreateAccountDto()
        {
          Address = "HCM",
          Email = "employee@gmail.com",
          FirstName = "Em",
          LastName = "Ployee",
          IndentityNumber = "2432513533",
          Phone = "0341231111",
          Balance = 0,
          Username = "employee"
        }
      );
    }
  }
}
