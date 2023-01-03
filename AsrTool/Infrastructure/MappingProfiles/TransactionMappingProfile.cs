using AsrTool.Dtos;
using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.Domain.Objects.Jobs;

namespace AsrTool.Infrastructure.MappingProfiles
{
    public class TransactionMappingProfile : BaseMappingProfile<Transaction>
    {
        public TransactionMappingProfile()
        {
            CreateMap<Transaction, TransactionDto>()
              .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(des => des.FromAccountNumber, opt => opt.MapFrom(src => src.FromAccountNumber))
              .ForMember(des => des.ToAccountNumber, opt => opt.MapFrom(src => src.ToAccountNumber))
              .ForMember(des => des.Amount, opt => opt.MapFrom(src => src.Amount))
              .ForMember(des => des.Type, opt => opt.MapFrom(src => src.Type));
        }
    }
}
