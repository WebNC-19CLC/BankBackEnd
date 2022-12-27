using AsrTool.Dtos;
using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.Domain.Objects.Jobs;

namespace AsrTool.Infrastructure.MappingProfiles
{
  public class AccountMappingProfile : BaseMappingProfile<BankAccount>
  {
    public AccountMappingProfile()
    {
      CreateMap<BankAccount, AccountDto>()
        .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(des => des.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
        .ForMember(des => des.Balance, opt => opt.MapFrom(src => src.Balance));

    }
  }
}
