using AsrTool.Dtos;
using AsrTool.Infrastructure.Domain.Entities;

namespace AsrTool.Infrastructure.MappingProfiles
{
  public class RoleMappingProfile : BaseMappingProfile<Role>
  {
    public RoleMappingProfile()
    {
      CreateMap<Role, RoleDto>()
        .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
        .ForMember(des => des.Rights, opt => opt.MapFrom(src => src.Rights))
        .ForMember(des => des.RowVersion, opt => opt.MapFrom(src => src.RowVersion));

      CreateMap<RoleDto, Role>()
        .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
        .ForMember(des => des.Rights, opt => opt.MapFrom(src => src.Rights))
        .ForMember(des => des.RowVersion, opt => opt.MapFrom(src => src.RowVersion));
    }
  }
}