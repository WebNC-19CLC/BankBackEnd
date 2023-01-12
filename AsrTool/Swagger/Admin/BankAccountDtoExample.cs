using Swashbuckle.AspNetCore.Filters;
using AsrTool.Dtos;

namespace AsrTool.Swagger.Admin
{
  public class BankAccountDtoExample : IMultipleExamplesProvider<BankAccountDto>
  {
    public IEnumerable<SwaggerExample<BankAccountDto>> GetExamples()
    {
      yield return SwaggerExample.Create(
       "Example",
       new BankAccountDto()
       {
         AccountNumber = "123456789",
         Name = "Anh Hoang",
         Password = "341311",
         Role = "User",
         Email = "employee@gmail.com",
         Username = "employee"
       }
     );
    }
  }
}
