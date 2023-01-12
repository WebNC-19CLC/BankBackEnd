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
    public class DeleteBankCommandHandler : IRequestHandler<DeleteBankCommand>
    {
        private readonly IAsrContext _asrContext;

        public DeleteBankCommandHandler(IAsrContext asrContext)
        {
            _asrContext = asrContext;
        }

        public async Task<Unit> Handle(DeleteBankCommand request, CancellationToken cancellationToken)
        {
            var bank = await _asrContext.Get<Domain.Entities.Bank>().SingleOrDefaultAsync(x => x.Id == request.id);
            if(bank == null)
            {
                throw new Exception("Bank is not existed");
            }
            await _asrContext.RemoveAsync(bank);
            await _asrContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
