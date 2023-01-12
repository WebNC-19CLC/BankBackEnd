using Swashbuckle.AspNetCore.Filters;
using AsrTool.Dtos;

namespace AsrTool.Swagger.Auth
{
  public class AuthExample : IMultipleExamplesProvider<LoginRequestDto>
  {

    public IEnumerable<SwaggerExample<LoginRequestDto>> GetExamples()
    {
      yield return SwaggerExample.Create(
          "Example",
          new LoginRequestDto
          {
            Username = "admin",
            Password = "password",
            RecaptchaToken = "string"
          }
        );
    }
  }

  public class RegisterRequestExample : IMultipleExamplesProvider<RegisterRequestDto>
  {
    public IEnumerable<SwaggerExample<RegisterRequestDto>> GetExamples()
    {
      yield return SwaggerExample.Create(
       "Example",
       new RegisterRequestDto()
       {
         Address = "HCM",
         Email = "employee@gmail.com",
         FirstName = "Em",
         LastName = "Ployee",
         IndentityNumber = "2432513533",
         Phone = "0341231111",
         Username = "employee",
         Password = "password",
       }
     );
    }
  }

  public class ForgotPasswordGenOTPDtoExample : IMultipleExamplesProvider<ForgotPasswordGenOTPDto>
  {
    public IEnumerable<SwaggerExample<ForgotPasswordGenOTPDto>> GetExamples()
    {
      yield return SwaggerExample.Create(
      "Example",
      new ForgotPasswordGenOTPDto()
      {
        Username = "admin"
      }
    );
    }
  }

  public class ValidateOTPForgotPasswordDtoExample : IMultipleExamplesProvider<ValidateOTPForgotPasswordDto>
  {
    public IEnumerable<SwaggerExample<ValidateOTPForgotPasswordDto>> GetExamples()
    {
      yield return SwaggerExample.Create(
       "Example",
       new ValidateOTPForgotPasswordDto()
       {
         Username = "admin",
         OTP = "243111"
       });
    }
  }

  public class ForgotPasswordDtoExample : IMultipleExamplesProvider<ForgotPasswordDto>
  {
    public IEnumerable<SwaggerExample<ForgotPasswordDto>> GetExamples()
    {
      yield return SwaggerExample.Create(
      "Example",
      new ForgotPasswordDto()
      {
        Username = "admin",
        Password = "password"
      });
    }
  }
}
