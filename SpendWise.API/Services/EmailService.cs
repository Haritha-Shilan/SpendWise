using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace SpendWise.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendWelcomeEmailAsync(string email, string fullName)
        {
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(_settings.SenderName, _settings.SenderEmail));

            message.To.Add(
                new MailboxAddress(fullName, email));

            message.Subject = "Welcome to SpendWise!";

            message.Body = new TextPart("plain")
            {
                Text = $"""
                    Hello {fullName},

                    Welcome to SpendWise!

                    Your account has been successfully created.
                    You can now start managing your personal finances with SpendWise.

                    Happy spending wisely!

                    Regards,
                    SpendWise Team
                    """
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _settings.SmtpHost,
                _settings.SmtpPort,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _settings.Username,
                _settings.Password);

            await smtp.SendAsync(message);

            await smtp.DisconnectAsync(true);
        }
    }
}