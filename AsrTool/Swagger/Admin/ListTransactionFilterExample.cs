using Swashbuckle.AspNetCore.Filters;
using AsrTool.Dtos;

namespace AsrTool.Swagger.Admin
{
  public class ListTransactionFilterExample : IMultipleExamplesProvider<ListTransactionFilter>
  {
    public IEnumerable<SwaggerExample<ListTransactionFilter>> GetExamples()
    {
      yield return SwaggerExample.Create(
       "Example",
       new ListTransactionFilter()
       {
         DateEnd = DateTime.Now,
         DateStart = DateTime.Now,
         BankDestinationId = null,
         BankSourceId = null,
         Type = "Transaction"
       }
     );
    }
  }
}
