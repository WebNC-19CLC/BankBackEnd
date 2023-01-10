using Swashbuckle.AspNetCore.Filters;
using AsrTool.Dtos;

namespace AsrTool.Swagger.Account
{
    public class NotificationExample : IMultipleExamplesProvider<NotifationDto>
    {
        public IEnumerable<SwaggerExample<NotifationDto>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Example",
                new NotifationDto
                {
                    Id = 1,
                    Time = DateTime.Now,
                    Description = "Description",
                    Type = "Client decide type",
                });
        }

        public class GetListNotificationExample : IMultipleExamplesProvider<List<NotifationDto>>{
            public IEnumerable<SwaggerExample<List<NotifationDto>>> GetExamples()
            {
                yield return SwaggerExample.Create(
                    "Example",
                    new List<NotifationDto>
                    {
                        new NotifationDto
                        {
                            Id = 1,
                            Time = DateTime.Now,
                            Description = "Transaction is completed",
                            Type = "Transaction",
                        },
                        new NotifationDto
                        {
                            Id = 2,
                            Time = DateTime.Now,
                            Description = "Paid me soon",
                            Type = "Debit",
                        },
                        new NotifationDto
                        {
                            Id = 3,
                            Time = DateTime.Now,
                            Description = "Write something here",
                            Type = "Client decide type",
                        }
                    });
            }
        }
    }
}
