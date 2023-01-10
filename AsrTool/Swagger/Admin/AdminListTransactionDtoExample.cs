using Swashbuckle.AspNetCore.Filters;
using AsrTool.Dtos;

namespace AsrTool.Swagger.Admin
{
  public class AdminListTransactionDtoExample : IMultipleExamplesProvider<AdminListTransactionDto>
  {
    public IEnumerable<SwaggerExample<AdminListTransactionDto>> GetExamples()
    {
      yield return SwaggerExample.Create(
        "Example",
        new AdminListTransactionDto()
        {
          TotalAmount = 20,
          TransactionList = new List<TransactionDto>() {
            new TransactionDto(){
              Id = 1,
              BankDestinationId = 1,
              BankSourceId = 1,
              Amount = 20,
              FromAccountNumber = "241311",
              ToAccountNumber = "124222",
              FromUser = "Anh",
              ToUser = "Tai",
              Time = DateTime.UtcNow,
              Type = "Transaction"
            }
          }
        }
      );
    }
  }
}
