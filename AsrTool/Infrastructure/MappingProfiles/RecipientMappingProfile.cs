using AsrTool.Dtos;
using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.Domain.Objects.Jobs;

namespace AsrTool.Infrastructure.MappingProfiles
{
  public class RecipientMappingProfile : BaseMappingProfile<Recipient>
  {
    public RecipientMappingProfile() {
      CreateMap<Recipient, RecipientDto>()
        .ForMember(des => des.AccountNumber, opt => opt.MapFrom(x => x.AccountNumber))
        .ForMember(des => des.SuggestedName, opt => opt.MapFrom(x => x.SuggestedName))
        .ForMember(des => des.BankDestinationId, opt => opt.MapFrom(x => x.BankDestinationId))
        .ForMember(des => des.Id, opt => opt.MapFrom(x => x.Id));
    }
  }
}
