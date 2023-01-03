using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Common.ThirdParty;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
    public class MakeTransactionCommandHandler : IRequestHandler<MakeTransactionCommand>
    {
        private readonly IAsrContext _asrContext;
        private readonly IUserResolver _userResolver;

        public MakeTransactionCommandHandler(IAsrContext asrContext, IUserResolver userResolver)
        {
            _asrContext = asrContext;
            _userResolver = userResolver;
        }

        public async Task<Unit> Handle(MakeTransactionCommand request, CancellationToken cancellationToken)
        {
            var from = await _asrContext.Get<Domain.Entities.BankAccount>().Include(x => x.OTP).SingleOrDefaultAsync(x => x.AccountNumber == request.MakeTransactionDto.FromAccountNumber);

            if (from == null)
            {
                throw new NotFoundException();
            }

            var to = await _asrContext.Get<Domain.Entities.BankAccount>().SingleOrDefaultAsync(x => x.AccountNumber == request.MakeTransactionDto.ToAccountNumber);

            if (to != null)
            {
                if (request.MakeTransactionDto.Amount > from.Balance)
                {
                    throw new Exception("Not enough balance");
                }

                if (request.MakeTransactionDto.ChargeReceiver)
                {
                    from.Balance = from.Balance - request.MakeTransactionDto.Amount;
                    to.Balance = to.Balance + request.MakeTransactionDto.Amount - Constants.Fee.TransactionFee;

                }
                else
                {
                    from.Balance = from.Balance - request.MakeTransactionDto.Amount - Constants.Fee.TransactionFee;
                    to.Balance = to.Balance + request.MakeTransactionDto.Amount;
                }

                from.OTP.Status = Domain.Enums.OTPStatus.Used;

                await _asrContext.UpdateAsync(from);
                await _asrContext.UpdateAsync(to);

                var trans = new Transaction
                {
                    FromId = from.Id,
                    FromAccountNumber = from.AccountNumber,
                    ToAccountNumber = to.AccountNumber,
                    ToId = to.Id,
                    Amount = request.MakeTransactionDto.Amount,
                    Type = request.MakeTransactionDto.Type,
                    Description = request.MakeTransactionDto.Description != null && request.MakeTransactionDto.Description != string.Empty ? request.MakeTransactionDto.Description : $"Account {_userResolver.CurrentUser.FullName} transfer {request.MakeTransactionDto.Amount} units",
                    ChargeReceiver = request.MakeTransactionDto.ChargeReceiver,
                    TransactionFee = Constants.Fee.TransactionFee,
                };

                await _asrContext.AddRangeAsync(trans);

                await _asrContext.SaveChangesAsync();
            }
            else if (request.MakeTransactionDto.BankId != null)
            {
                var bank = await _asrContext.Get<Domain.Entities.Bank>().SingleOrDefaultAsync(x => x.Id == request.MakeTransactionDto.BankId);

                if (bank == null)
                {
                    throw new NotFoundException<Domain.Entities.Bank>(request.MakeTransactionDto.BankId.Value);
                }

                IThirdPartyRequestHandler RequestHandler = SelectRequestHanlder(bank);

                TransactionDto assosiatedBankTransaction =  await CommandMakeTransactionInAssociatedBank(RequestHandler, request.MakeTransactionDto);
                
                await CommandCompleteTransaction(RequestHandler,assosiatedBankTransaction.Id.ToString());

                from.Balance = from.Balance - request.MakeTransactionDto.Amount - Constants.Fee.TransactionFee;

                await _asrContext.UpdateAsync(from);

                var trans = new Transaction
                {
                    FromId = from.Id,
                    FromAccountNumber = from.AccountNumber,
                    ToAccountNumber = request.MakeTransactionDto.ToAccountNumber,
                    ToId = null,
                    Amount = request.MakeTransactionDto.Amount,
                    Type = request.MakeTransactionDto.Type,
                    Description = request.MakeTransactionDto.Description != null && request.MakeTransactionDto.Description != string.Empty ? request.MakeTransactionDto.Description : $"Account {_userResolver.CurrentUser.FullName} transfer {request.MakeTransactionDto.Amount} units",
                    ChargeReceiver = false,
                    TransactionFee = Constants.Fee.TransactionFee,
                };

                await _asrContext.AddRangeAsync(trans);

                await _asrContext.SaveChangesAsync();
            }
            return Unit.Value;

        }

        private async Task<TransactionDto> CommandMakeTransactionInAssociatedBank(IThirdPartyRequestHandler requestHandler,MakeTransactionDto makeTransaction)
        {
            var client = requestHandler.CommandMakeTransaction(makeTransaction);

            var bodyJson = JsonConvert.SerializeObject(makeTransaction);
            var payload = new StringContent(bodyJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respone = await client.PostAsync("", payload);

            if (!respone.IsSuccessStatusCode)
                throw new BusinessException("Failed to make transaction");

            var responseObject = respone.Content.ReadAsStringAsync().Result;

            TransactionDto transaction = JsonConvert.DeserializeObject<TransactionDto>(responseObject);

            // Valid signature 
            var isValidSignature = true;

            if (!isValidSignature)
            {
                throw new BusinessException("Failed to make transaction");
            }
            return transaction;
        }

        private async Task CommandCompleteTransaction(IThirdPartyRequestHandler requestHandler, string transactionId)
        {
            var client = requestHandler.CommandCompleteTransaction(transactionId);

            HttpResponseMessage respone = await client.PutAsync("",null);

            if (!respone.IsSuccessStatusCode)
                throw new BusinessException("Failed to make transaction");
        }


        public IThirdPartyRequestHandler SelectRequestHanlder(Domain.Entities.Bank bank)
        {
            switch (bank.Name)
            {
                case "bank1":
                    return new RSABankRequestHandler(bank);
                case "bank2":
                    return new RSABankRequestHandler(bank);
                default:
                    return new RSABankRequestHandler(bank);
            }
        }
    }
}
