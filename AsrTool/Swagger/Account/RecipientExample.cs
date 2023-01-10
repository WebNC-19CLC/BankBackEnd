using Swashbuckle.AspNetCore.Filters;
using AsrTool.Dtos;

namespace AsrTool.Swagger.Account
{
    public class RecipientExample : IMultipleExamplesProvider<RecipientDto>
    {

        public class ListRecipientExample : IMultipleExamplesProvider<List<RecipientDto>>
        {
            public IEnumerable<SwaggerExample<List<RecipientDto>>> GetExamples()
            {
                yield return SwaggerExample.Create(
                  "Example",
                  new List<RecipientDto>
                  {
                  new RecipientDto
                  {
                      AccountNumber = "123456",
                      BankDestinationId =null,
                      Id = 1,
                      SuggestedName = "Nguyen Van B"
                  },
                  new RecipientDto
                  {
                      AccountNumber = "198762",
                      BankDestinationId =null,
                      Id = 2,
                      SuggestedName = "Nguyen Van C"
                  },
                  new RecipientDto
                  {
                      AccountNumber = "234567",
                      BankDestinationId =1,
                      Id = 3,
                      SuggestedName = "Tran Van A"
                  },
                  new RecipientDto
                  {
                      AccountNumber = "345678",
                      BankDestinationId =2,
                      Id = 4,
                      SuggestedName = "Pham Van A"
                  },
                  }
                );
            }
        }


        public class CreateRecipientRequestExample : IMultipleExamplesProvider<CreateRecipientDto>
        {
            public IEnumerable<SwaggerExample<CreateRecipientDto>> GetExamples()
            {
                yield return SwaggerExample.Create(
                  "With Suggested Name",
                  new CreateRecipientDto
                  {
                      AccountNumber = "345678",
                      BankDestinationId = null,
                      SuggestedName = "MyGod",
                  }
                );

                yield return SwaggerExample.Create(
                   "Without Suggested Name",
                   new CreateRecipientDto
                   {
                       AccountNumber = "345678",
                       BankDestinationId = null,
                       SuggestedName = null,
                   }
                 );
            }
        }

        public class CreateRecipientResponseExample : IMultipleExamplesProvider<RecipientDto>
        {
            public IEnumerable<SwaggerExample<RecipientDto>> GetExamples()
            {
                yield return SwaggerExample.Create(
                  "With Suggested Name",
                  new RecipientDto
                  {
                      Id = 1,
                      AccountNumber = "345678",
                      BankDestinationId = null,
                      SuggestedName = "MyGod",
                  }
                );

                yield return SwaggerExample.Create(
                   "Without Suggested Name",
                   new RecipientDto
                   {
                       Id = 1,
                       AccountNumber = "345678",
                       BankDestinationId = null,
                       SuggestedName = "Nguyen Van B",
                   }
                 );
            }
        }

        public IEnumerable<SwaggerExample<RecipientDto>> GetExamples()
        {
            yield return SwaggerExample.Create(
              "Example", 
              new RecipientDto
              {
                  AccountNumber = "345678",
                  BankDestinationId = 2,
                  Id = 4,
                  SuggestedName = "Pham Van A"
              }
            );
        }
    }
}
