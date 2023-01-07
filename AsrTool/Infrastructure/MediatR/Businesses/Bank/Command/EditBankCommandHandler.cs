using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using MediatR;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using AsrTool.Infrastructure.Helpers;
using AsrTool.Infrastructure.Domain.Entities;

namespace AsrTool.Infrastructure.MediatR.Businesses.Bank.Command
{
    public class EditBankCommandHandler : IRequestHandler<EditBankCommand, CreateBankDto>
    {
        private readonly IAsrContext _asrContext;

        public EditBankCommandHandler(IAsrContext asrContext)
        {
            _asrContext = asrContext;
        }

        public async Task<CreateBankDto> Handle(EditBankCommand request, CancellationToken cancellationToken)
        {
            var bank = await _asrContext.Get<Domain.Entities.Bank>().SingleOrDefaultAsync(x => x.Name == request.Request.Name);
            if(bank == null)
            {
                throw new Exception("Bank is not existed");
            }

            bank.EncryptRsaPublicKey = request.Request.HashAndAsymmetricEncryptionKey;

            await _asrContext.UpdateAsync(bank);
            await _asrContext.SaveChangesAsync();

            return new CreateBankDto
            {
                Name = bank.Name,
                HashAndAsymmetricEncryptionKey = bank.DecryptPublicKey,
            };
        }
    }
}
