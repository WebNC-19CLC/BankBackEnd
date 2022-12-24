using AsrTool.Dtos;
using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Helpers;

namespace AsrTool.Infrastructure.MappingProfiles
{
  public class UserMappingProfile : BaseMappingProfile<User>
  {
    public UserMappingProfile()
    {
      CreateMap<IUser, UserDto>()
        .ForMember(dst => dst.EmployeeId, opt => opt.MapFrom(src => src.Id))
        .ForMember(dst => dst.IdentityName, opt => opt.MapFrom(src => src.Username))
        .ForMember(dst => dst.FirstName, opt => opt.MapFrom(src => src.FirstName))
        .ForMember(dst => dst.LastName, opt => opt.MapFrom(src => src.LastName))
        .ForMember(dst => dst.Visa, opt => opt.MapFrom(src => src.Visa))
        .ForMember(dst => dst.Rights, opt => opt.MapFrom(src => src.Rights.Select(x => x)))
        .ForMember(dst => dst.FullName, opt => opt.MapFrom(src => src.FullName))
        .ForMember(dst => dst.LegalUnit, opt => opt.MapFrom(src => src.LegalUnit))
        .ForMember(dst => dst.Site, opt => opt.MapFrom(src => src.Site))
        .ForMember(dst => dst.OrganizationUnit, opt => opt.MapFrom(src => src.OrganizationUnit))
        .ForMember(dst => dst.Level, opt => opt.MapFrom(src => src.Level))
        .ForMember(dst => dst.AuthType, opt => opt.MapFrom(src => src.AuthType))
        .ForMember(dst => dst.ImageSmall, opt => opt.Ignore())
        .ForMember(dst => dst.RoleName, opt => opt.MapFrom(src => src.RoleName))
        .ForMember(dst => dst.TimeZoneId, opt => opt.MapFrom(src => src.TimeZoneId));

      CreateMap<Employee, UserRefDto>()
        .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(des => des.RowVersion, opt => opt.MapFrom(src => src.RowVersion))
        .ForMember(des => des.FullName, opt => opt.MapFrom(src => StringHelper.GetFullName(src.FirstName, src.LastName)))
        .ForMember(des => des.Visa, opt => opt.MapFrom(src => src.Visa))
        .ForMember(des => des.RoleId, opt => opt.MapFrom(src => src.RoleId));
    }
  }
}