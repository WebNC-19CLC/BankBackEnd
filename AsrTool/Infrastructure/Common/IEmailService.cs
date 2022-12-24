using System.Net.Mail;

namespace AsrTool.Infrastructure.Common
{
  public interface IEmailService
  {
    void Send(string subject, string body, IEnumerable<string> toRecipients, IEnumerable<string> ccRecipients = null, bool isHtml = true);

    void Send(ICollection<MailMessage> messages);

    MailMessage GetMailMessage(string subject, string body, IEnumerable<string> toRecipients, IEnumerable<string> ccRecipients = null, bool isHtml = true);
  }
}
