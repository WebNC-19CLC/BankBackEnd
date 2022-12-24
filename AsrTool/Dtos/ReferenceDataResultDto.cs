using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.Dtos
{
    public class ReferenceDataResultDto
    {
        public ReferenceDataType Type { get; set; }

        public IEnumerable<ReferenceDataDto> Data { get; set; }
    }
}
