using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Domain.Entities.APIReponse;
using AsrTool.Infrastructure.Domain.Objects.Configurations;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Commands
{
  public class LoginCommandHandler : IRequestHandler<LoginCommand>
  {
    private readonly IUserManager _userManager;
    private readonly AppSettings _appSettings;

    public LoginCommandHandler(IUserManager userManager, IOptions<AppSettings> settings)
    {
      _userManager = userManager;
      _appSettings = settings.Value;
    }

    public async Task<Unit> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
      if (!Constants.TestAccounts.Contains(request.Request.Username))
      {
        var dictionary = new Dictionary<string, string> { { "secret", _appSettings.RecaptchaSecretKey }, { "response", request.Request.RecaptchaToken } };
        var postContent = new FormUrlEncodedContent(dictionary);
        HttpResponseMessage recaptchaResponse = null;
        string stringContent = "";
        using (var http = new HttpClient()) {
          recaptchaResponse = await http.PostAsync("https://www.google.com/recaptcha/api/siteverify", postContent);
          stringContent = await recaptchaResponse.Content.ReadAsStringAsync();
        }
        if (!recaptchaResponse.IsSuccessStatusCode)
        {
          throw new BusinessException("Unable to verify recaptcha token");
        }
        if (string.IsNullOrEmpty(stringContent))
        {
          throw new BusinessException("Invalid reCAPTCHA verification response");
        }
        var googleReCaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(stringContent);
        if (!googleReCaptchaResponse.Success) {
          throw new BusinessException("Google reCAPTCHA authentication fail");
        }
      }

      var normalizedUserName = request.Request.Username?.Split(new string[] { "\\", "/" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
      await _userManager.SignIn(request.HttpContext, normalizedUserName, request.Request.Password);
      return Unit.Value;
    }
  }
}