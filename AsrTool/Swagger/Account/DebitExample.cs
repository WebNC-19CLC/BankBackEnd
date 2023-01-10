using Swashbuckle.AspNetCore.Filters;
using AsrTool.Dtos;

namespace AsrTool.Swagger.Account
{
    public class DebitExample : IMultipleExamplesProvider<DebitDto>
    {
        public IEnumerable<SwaggerExample<DebitDto>> GetExamples()
        {
            DateTime time = DateTime.Now;
            time.AddMonths(2);
            yield return SwaggerExample.Create(
                "Other Debt",
                new DebitDto
                {
                    Id = 1,
                    FromAccountNumber = "123123",
                    FromUser = "Nguyen Van A",
                    ToAccountNumber = "321321",
                    ToUser = "Nguyen Van B",
                    BankDestinationId = null,
                    BankSourceId = null,
                    Amount = 100,
                    Time = DateTime.Now,
                    DateDue = time,
                    IsPaid = false,
                });
            yield return SwaggerExample.Create(
                "Self Debt",
                new DebitDto
                {
                    Id = 2,
                    FromAccountNumber = "321321",
                    FromUser = "Nguyen Van B",
                    ToAccountNumber = "123123",
                    ToUser = "Nguyen Van A",
                    BankDestinationId = null,
                    BankSourceId = null,
                    Amount = 100,
                    Time = DateTime.Now,
                    DateDue = time,
                    IsPaid = false,
                });
        }

        public class GetListDebitResponseExample : IMultipleExamplesProvider<List<DebitDto>>
        {
            public IEnumerable<SwaggerExample<List<DebitDto>>> GetExamples()
            {
                yield return SwaggerExample.Create(
                "Example",
                new List<DebitDto>
                {
                    new DebitDto
                    {
                        Id = 0,
                        FromAccountNumber = "123123",
                        FromUser = "Nguyen Van A",
                        ToAccountNumber = "321321",
                        ToUser = "Nguyen Van B",
                        BankDestinationId = null,
                        BankSourceId = null,
                        Amount = 100,
                        Time = DateTime.Now,
                        DateDue = DateTime.Now,
                        IsPaid = true,
                    },
                    new DebitDto
                    {
                        Id = 0,
                        FromAccountNumber = "123123",
                        FromUser = "Nguyen Van A",
                        ToAccountNumber = "321321",
                        ToUser = "Nguyen Van B",
                        BankDestinationId = null,
                        BankSourceId = null,
                        Amount = 200,
                        Time = DateTime.Now,
                        DateDue = DateTime.Now,
                        IsPaid = false,
                    },
                    new DebitDto
                    {
                        Id = 0,
                        FromAccountNumber = "321321",
                        FromUser = "Nguyen Van B",
                        ToAccountNumber = "123123",
                        ToUser = "Nguyen Van A",
                        BankDestinationId = null,
                        BankSourceId = null,
                        Amount = 300,
                        Time = DateTime.Now,
                        DateDue = DateTime.Now,
                        IsPaid = false,
                    },
                });
            }

        }

        public class CreateDebitRequest : IMultipleExamplesProvider<CreateDebitDto>
        {
            public IEnumerable<SwaggerExample<CreateDebitDto>> GetExamples()
            {
                DateTime time = DateTime.Now;
                time.AddMonths(2);
                yield return SwaggerExample.Create(
                    "Self Debt",
                    new CreateDebitDto
                    {
                        AccountNumber = "123123",
                        BankDestinationId = null,
                        Amount = 100,
                        DateDue = time,
                        Description = "Paid Me",
                        SelfInDebt = false
                    });

                yield return SwaggerExample.Create(
                    "Other Detb",
                    new CreateDebitDto
                    {
                        AccountNumber = "321321",
                        BankDestinationId = null,
                        Amount = 100,
                        DateDue = time,
                        Description = "Me Paid",
                        SelfInDebt = true
                    });
            }
        }

        public class CancelDebitRequest : IMultipleExamplesProvider<DeleteDebitRequestDto>
        {
            public IEnumerable<SwaggerExample<DeleteDebitRequestDto>> GetExamples()
            {
                yield return SwaggerExample.Create(
                    "Example",
                    new DeleteDebitRequestDto
                    {
                        Description = "Cancel debit reason",
                        Id = 1,
                    }
                    );
            }
        }

        public class PayDebitRequest : IMultipleExamplesProvider<PayDebitDto>
        {
            public IEnumerable<SwaggerExample<PayDebitDto>> GetExamples()
            {
                yield return SwaggerExample.Create(
                    "Example",
                    new PayDebitDto
                    {
                        Id =1,
                        OTP ="123123"
                    }
                    );
            }
        }
    }
}
