using Swashbuckle.AspNetCore.Filters;
using AsrTool.Dtos;

namespace AsrTool.Swagger.Admin
{
  public class SetActiveStatusDtoExample : IMultipleExamplesProvider<SetActiveStatusDto>
  {
    public IEnumerable<SwaggerExample<SetActiveStatusDto>> GetExamples()
    {
      yield return SwaggerExample.Create(
       "Example",
       new SetActiveStatusDto()
       {
         BankAccountId = 1,
         IsActive = true,
       }
     );
    }
  }
}
