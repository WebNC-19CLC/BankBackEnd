using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using MediatR;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using AsrTool.Infrastructure.Helpers;

namespace AsrTool.Infrastructure.MediatR.Businesses.Bank.Command
{
    public class CreateBankCommandHandle : IRequestHandler<CreateBankCommand, BankDto>
    {
        private readonly IAsrContext _asrContext;

        public CreateBankCommandHandle(IAsrContext asrContext)
        {
            _asrContext = asrContext;
        }

        public async Task<BankDto> Handle(CreateBankCommand request, CancellationToken cancellationToken)
        {
            var isBankExist = await _asrContext.Get<Domain.Entities.Bank>().AnyAsync(x => x.Name == request.Request.Name);
            if(isBankExist)
            {
                throw new Exception("Bank name is already existed");
            }

            var csp = new RSACryptoServiceProvider(1024);
            var privKey = csp.ExportParameters(true);
            var pubKey = csp.ExportParameters(false);


            var newBank = new Domain.Entities.Bank
            {
                Name = request.Request.Name,
                API = request.Request.API,
                DecryptPublicKey = EncryptionHelper.ConvertRSAKeyToString(pubKey),
                DecryptRsaPrivateKey = EncryptionHelper.ConvertRSAKeyToString(privKey),
                EncryptRsaPublicKey = "",
            };

            await _asrContext.AddAsync(newBank);
            await _asrContext.SaveChangesAsync();

            return new BankDto
            {
                Name = newBank.Name
            };
        }
    }
}
