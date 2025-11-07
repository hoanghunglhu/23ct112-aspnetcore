using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace LearnApiNetCore.Services
{
    public class EmailSettings
    {
        public string From { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
    }

    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtp = new SmtpClient
            {
                Host = _emailSettings.Host,
                Port = _emailSettings.Port,
                EnableSsl = true,
                Credentials = new NetworkCredential(_emailSettings.From, _emailSettings.Password)
            };

            var message = new MailMessage(_emailSettings.From, to, subject, body)
            {
                IsBodyHtml = true
            };

            await smtp.SendMailAsync(message);
        }
    }
}
