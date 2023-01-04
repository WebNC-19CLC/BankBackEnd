using AsrTool.Infrastructure.Domain.Entities;

namespace AsrTool.Infrastructure.Common.ThirdParty
{
    public class ThirdPartyRequestHandlerFactory
    {
        public static IThirdPartyRequestHandler GetThirdPartyRequestHandler(Bank bank)
        {
            switch (bank.Name)
            {
                case Constants.AssociatedBank.RSA_BANK_NAME:
                    return new RSABankRequestHandler(bank);
                case Constants.AssociatedBank.PGP_BANK_NAME:
                    return new RSABankRequestHandler(bank);
                default:
                    return new RSABankRequestHandler(bank);
            }
        }
    }
}
