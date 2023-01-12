using Swashbuckle.AspNetCore.Filters;
using AsrTool.Dtos;

namespace AsrTool.Swagger.Account
{
        public class TransactionExample : IMultipleExamplesProvider<MakeTransactionDto>
    {
        public IEnumerable<SwaggerExample<MakeTransactionDto>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Make Transaction Bank1",
                new MakeTransactionDto { 
                Amount = 500,
                BankId = 1,
                ChargeReceiver = false,
                Description = "Description",
                FromAccountNumber = "Account number in Bank Call API",
                ToAccountNumber = "Account number from my bank",
                Type = "MakeTransaction",
                }
                );
        }

        public class TranferMoney : IMultipleExamplesProvider<SelfTransferDto>
        {
            public IEnumerable<SwaggerExample<SelfTransferDto>> GetExamples()
            {
                yield return SwaggerExample.Create(
                  "In Bank",
                  new SelfTransferDto
                  {
                      Amount = 5,
                      BankId = null,
                      Description = "take my money",
                      OTP = "23456",
                      ChargeReceiver = true,
                      ToAccountNumber = "123456",
                  }
                 );

                yield return SwaggerExample.Create(
                  "Associate Bank",
                  new SelfTransferDto
                  {
                      Amount = 5,
                      BankId = 1,
                      Description = "take my money",
                      OTP = "23456",
                      ChargeReceiver = false,
                      ToAccountNumber = "123456",
                  }
                 );
            }
        }

        public class ListAccountTransactionExample : IMultipleExamplesProvider<List<TransactionDto>>
        {
            public IEnumerable<SwaggerExample<List<TransactionDto>>> GetExamples()
            {
                yield return SwaggerExample.Create(
                  "Example",
                  new List<TransactionDto>() {
                new TransactionDto()
                {
                    Id =1,
                    FromAccountNumber = "156311",
                    FromUser = "Nguyen Van A",
                    ToAccountNumber = "156322",
                    ToUser = "Nguyen Van B",
                    Type = "Transaction",
                    Amount = 3,
                    BankDestinationId = null,
                    BankSourceId = null,
                    Time = DateTime.Now,
                },
                new TransactionDto()
                {
                    Id =2,
                    ToAccountNumber = "156311",
                    ToUser = "Nguyen Van A",
                    Type = "Receive",
                    Amount = 5,
                    BankDestinationId = null,
                    BankSourceId = null,
                    Time = DateTime.Now,
                },
                new TransactionDto()
                {
                    Id =3,
                    ToAccountNumber = "156312",
                    ToUser = "Nguyen Van A",
                    FromAccountNumber = "218994",
                    FromUser = "Nguyen Van B",
                    Type = "Transaction",
                    Amount = 5,
                    BankDestinationId = null,
                    BankSourceId = 1,
                    Time = DateTime.Now,
                },
                 new TransactionDto()
                {
                    Id =4,
                    FromAccountNumber = "156312",
                    FromUser = "Nguyen Van A",
                    ToAccountNumber = "218994",
                    ToUser = "Nguyen Van B",
                    Type = "Transaction",
                    Amount = 6,
                    BankDestinationId = 1,
                    BankSourceId = null,
                    Time = DateTime.Now,
                },
                  }
                );
            }
        }
    }
}
