using Swashbuckle.AspNetCore.Filters;
using AsrTool.Dtos;

namespace AsrTool.Swagger.Account
{
    public class AccountExample : IMultipleExamplesProvider<BankAccountDto>
    {
        IEnumerable<SwaggerExample<BankAccountDto>> IMultipleExamplesProvider<BankAccountDto>.GetExamples()
        {
            yield return SwaggerExample.Create(
                "Example",
            new BankAccountDto
            {
                AccountNumber = "123456",
                Name = "Nguyen Van A",
                Password = "123123abc",
                Role = "User",
                Email = "customer@email.com",
                Username = "chetah"
            });
        }

        public class ChangePasswordRequestExample : IMultipleExamplesProvider<ChangePasswordDto>
        {
            public IEnumerable<SwaggerExample<ChangePasswordDto>> GetExamples()
            {
                yield return SwaggerExample.Create(
                    "Change Password Request Example",
                new ChangePasswordDto
                {
                    NewPassword = "yournewpassword",
                    Password = "youroldpassword"
                });
            }
        }
    }
}
