using System.Net;
using System.Net.Mail;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Domain.Objects.Configurations;
using Microsoft.Extensions.Options;

namespace AsrTool.Infrastructure.Common.Imp
{
  public class EmailService : IEmailService
  {
    private const string MAIL_FOLDER = @"C:\AsrToolMails";
    private readonly ILogger<EmailService> _logger;
    private readonly IUserResolver _userResolver;
    private readonly MailSettings _mailSettings;

    public EmailService(ILogger<EmailService> logger, IOptions<AppSettings> settings, IUserResolver userResolver)
    {
      _logger = logger;
      _userResolver = userResolver;
      _mailSettings = settings.Value.MailSettings;
    }

    public void Send(string subject, string body, IEnumerable<string> toRecipients, IEnumerable<string> ccRecipients = null, bool isHtml = true)
    {
      using var smtp = OpenSmtp();
      var message = GetMailMessage(subject, body, toRecipients, ccRecipients, isHtml);
      smtp.Send(message);
      _logger.LogInformation($"Email \"{subject}\" has been sent to \"{string.Join(", ", toRecipients)}\" by user \"{_userResolver.CurrentUser.FullName}\"");
    }

    public void Send(ICollection<MailMessage> messages)
    {
      using var smtp = OpenSmtp();
      foreach (var message in messages)
      {
        smtp.Send(message);
        _logger.LogInformation(
          $"Email \"{message.Subject}\" has been sent to \"{string.Join(", ", message.To.Select(x => x.Address))}\" by user \"{_userResolver.CurrentUser.FullName}\"");
      }
    }

    public MailMessage GetMailMessage(string subject, string body, IEnumerable<string> toRecipients, IEnumerable<string> ccRecipients = null, bool isHtml = true)
    {
      var message = new MailMessage
      {
        From = new MailAddress(_mailSettings.From),
        Subject = subject,
        Body = body,
        IsBodyHtml = isHtml
      };

      toRecipients.ToList().ForEach(x => message.To.Add(x));
      ccRecipients?.ToList().ForEach(x => message.CC.Add(x));

      return message;
    }

    private SmtpClient OpenSmtp()
    {
      var mailServer = _mailSettings.Server;
      var smtp = new SmtpClient(mailServer, _mailSettings.Port);
      if (!string.IsNullOrEmpty(_mailSettings.UserName))
      {
        smtp.Credentials = new NetworkCredential(_mailSettings.UserName, _mailSettings.Password);
      }

      if (string.IsNullOrEmpty(mailServer))
      {
        smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
        if (!Directory.Exists(MAIL_FOLDER))
        {
          Directory.CreateDirectory(MAIL_FOLDER);
        }
        smtp.PickupDirectoryLocation = MAIL_FOLDER;
      }

      return smtp;
    }
  }
}
