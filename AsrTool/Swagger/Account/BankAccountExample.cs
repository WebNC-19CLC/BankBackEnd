using Swashbuckle.AspNetCore.Filters;
using AsrTool.Dtos;
using System.Runtime.CompilerServices;

namespace AsrTool.Swagger.Account
{
    public class BankAccountExample : IMultipleExamplesProvider<AccountDto>
    {
        public IEnumerable<SwaggerExample<AccountDto>> GetExamples()
        {
            yield return SwaggerExample.Create(
              "Example",
              new AccountDto()
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
              }
            );
        }

        public class SearchAccountRequest : IMultipleExamplesProvider<SearchAccountRequestDto>
        {
            public IEnumerable<SwaggerExample<SearchAccountRequestDto>> GetExamples()
            {
                yield return SwaggerExample.Create(
                    "In Bank Account",
                    new SearchAccountRequestDto
                    {
                        AccountNumber = "123123",
                        BankId = null,
                    }
                    );

                yield return SwaggerExample.Create(
                    "Other Bank Account",
                    new SearchAccountRequestDto
                    {
                        AccountNumber = "234234",
                        BankId = 1,
                    }
                    );
            }
        }

        public class SearchAccountResponse : IMultipleExamplesProvider<ShortAccountDto>
        {
            public IEnumerable<SwaggerExample<ShortAccountDto>> GetExamples()
            {
                yield return SwaggerExample.Create(
                    "In Bank Account",
                    new ShortAccountDto
                    {
                        AccountNumber = "123123",
                        FullName = "Nguyen Van A",
                        BankAccountId = null,
                    });

                yield return SwaggerExample.Create(
                    "Other Bank Account",
                    new ShortAccountDto
                    {
                        AccountNumber = "234234",
                        FullName = "Nguyen Van B",
                        BankAccountId = null,
                    });
            }
        }
    }
}
