using AsrTool.Dtos;
using MediatR;
using System.Collections;

namespace AsrTool.Infrastructure.MediatR.Businesses.Bank.Queries
{
    public class GetAllBankQuery :IRequest<ICollection<BankDto>>
    {

    }
}
