using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NetBank.Core.Application.Dtos.Email;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Domain.Settings;

namespace NetBank.Infraestructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            emailSettings = options.Value;
        }
        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                MimeMessage email = new();
                email.Sender = MailboxAddress.Parse(emailSettings.EmailFrom);
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                BodyBuilder bodyBuilder = new();
                bodyBuilder.HtmlBody = request.Body;
                email.Body = bodyBuilder.ToMessageBody();

                using SmtpClient smtp = new SmtpClient();
                await smtp.ConnectAsync(emailSettings.SmtpHost, emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(emailSettings.SmtpUser, emailSettings.SmtpPass);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
