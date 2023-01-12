using Swashbuckle.AspNetCore.Filters;
using AsrTool.Dtos;

namespace AsrTool.Swagger.Employee
{
  public class EmployeeExample
  {
  }

  public class ListAccountDtoExample : IMultipleExamplesProvider<List<AccountDto>>
  {
    public IEnumerable<SwaggerExample<List<AccountDto>>> GetExamples()
    {
      yield return SwaggerExample.Create(
             "Example",
              new List<AccountDto>() { new AccountDto()
             {
               BankAccountId = 2,
               AccountNumber = "156311",
               FullName = "Nguyen Van A",
               Balance = 3200000,
               Email = "nguyenvana@email.com",
               Role = "User",
               IsActive = true,
               Recipients = new List<RecipientDto>()
                 {
                new RecipientDto()
                {
                    Id = 1,
                    AccountNumber = "128991",
                    BankDestinationId = null,
                },
                new RecipientDto()
                {
                    Id = 2,
                    AccountNumber = "118992",
                    BankDestinationId = null,
                },
                 new RecipientDto()
                {
                    Id = 3,
                    AccountNumber = "518992",
                    BankDestinationId = 1,
                },
                 new RecipientDto()
                {
                    Id = 4,
                    AccountNumber = "618992",
                    BankDestinationId = 2,
                },
                 }
             } }
           );
    }
  }

  public class ChargeMoneyToAccountDtoExample : IMultipleExamplesProvider<ChargeMoneyToAccountDto>
  {
    public IEnumerable<SwaggerExample<ChargeMoneyToAccountDto>> GetExamples()
    {
      yield return SwaggerExample.Create(
                   "Example",
                   new ChargeMoneyToAccountDto
                   {
                     AccountNumber = "123123",
                     Amount = 100,
                   }
                   );
    }
  }

  public class MakeTransactionDtoExample : IMultipleExamplesProvider<MakeTransactionDto>
  {
    public IEnumerable<SwaggerExample<MakeTransactionDto>> GetExamples()
    {
      yield return SwaggerExample.Create(
                   "Example",
                   new MakeTransactionDto
                   {
                     FromAccountNumber = "123123",
                     ToAccountNumber = "245678",
                     Description = "Example",
                     BankId = null,
                     ChargeReceiver = true,
                     Type = "Transaction",
                     Amount = 100,
                   }
                   );
    }
  }


}
