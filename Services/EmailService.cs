using System.Net;
using System.Net.Mail;
using LearnApiNetCore.Models;
using Microsoft.Extensions.Options;
using NLog;

namespace LearnApiNetCore.Services
{
    public class EmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var client = new SmtpClient(_smtpSettings.Host)
                {
                    Port = _smtpSettings.Port,
                    Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password),
                    EnableSsl = _smtpSettings.EnableSsl
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.UserName, "Viên Xuân Quý"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
                _logger.Info($"Email đã gửi đến {toEmail}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Lỗi khi gửi email");
                return false;
            }
        }
    }
}
