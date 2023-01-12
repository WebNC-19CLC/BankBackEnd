using Swashbuckle.AspNetCore.Filters;
using AsrTool.Dtos;

namespace AsrTool.Swagger.Bank
{
    public class BankExample : IMultipleExamplesProvider<CreateBankDto>
    {
        public IEnumerable<SwaggerExample<CreateBankDto>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Bank1",
                new CreateBankDto
                {
                    Name = "Bank1",
                    HashAndAsymmetricEncryptionKey = "Key",
                });
            yield return SwaggerExample.Create(
                "Bank2",
                new CreateBankDto
                { 
                Name = "Bank2",
                HashAndAsymmetricEncryptionKey = "Key",
                });
        }
    }

    public class GetListBankResponse : IMultipleExamplesProvider<List<BankDto>>
    {
        public IEnumerable<SwaggerExample<List<BankDto>>> GetExamples()
        {
            yield return SwaggerExample.Create("List Bank Response",
                new List<BankDto>
                {
                    new BankDto
                    {
                        Id = 1,
                        Name = "RSABank",
                    },
                    new BankDto
                    {
                        Id = 2,
                        Name = "PGPBank",
                    }
                });
        }
    }

    public class CreateBankRequestExample : IMultipleExamplesProvider<CreateBankDto>
    {
        public IEnumerable<SwaggerExample<CreateBankDto>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Bank1",
                new CreateBankDto
                {
                    Name = "Bank1",
                    HashAndAsymmetricEncryptionKey = "Associated bank Hash,Encryption key"
                });

            yield return SwaggerExample.Create(
                "Bank2",
                new CreateBankDto
                {
                    Name = "Bank2",
                    HashAndAsymmetricEncryptionKey = "Associated bank Hash,Encryption key"
                });
        }
    }
}
