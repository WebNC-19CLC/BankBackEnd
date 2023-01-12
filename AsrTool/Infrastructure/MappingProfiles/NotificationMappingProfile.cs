using AsrTool.Dtos;
using AsrTool.Infrastructure.Domain.Entities;

namespace AsrTool.Infrastructure.MappingProfiles
{
  public class NotificationMappingProfile : BaseMappingProfile<Notification>
  {
    public NotificationMappingProfile() {
      CreateMap<Notification, NotifationDto>()
        .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(des => des.Description, opt => opt.MapFrom(src => src.Description))
        .ForMember(des => des.Time, opt => opt.MapFrom(src => src.CreatedOn))
        .ForMember(des => des.Type, opt => opt.MapFrom(src => src.Type != null ? src.Type : "Debit"));
    }
  }
}
