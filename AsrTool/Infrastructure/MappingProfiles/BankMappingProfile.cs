using AsrTool.Dtos;
using AsrTool.Infrastructure.Domain.Entities;

namespace AsrTool.Infrastructure.MappingProfiles
{
  public class BankMappingProfile : BaseMappingProfile<Bank>
  {
    public BankMappingProfile()
    {
        CreateMap<Bank, BankDto>()
                .ForMember(des => des.Name, otp => otp.MapFrom(src => src.Name));

    }
  }
}