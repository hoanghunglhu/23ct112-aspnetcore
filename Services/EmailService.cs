using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using LearnApiNetCore.Services;
using System.Configuration;

namespace LearnApiNetCore.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogService _logger;

        public EmailService(IConfiguration config, ILogService logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody, List<string>? attachmentPaths = null)
            => await SendEmailAsync(new List<string> { toEmail }, subject, htmlBody, attachmentPaths);

        public async Task SendEmailAsync(List<string> toEmails, string subject, string htmlBody, List<string>? attachmentPaths = null)
        {
            var settings = _config.GetSection("EmailSettings");
            try
            {
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(settings["FromEmail"], settings["FromName"]);
                foreach (var to in toEmails)
                    mailMessage.To.Add(to);

                mailMessage.Subject = subject;
                mailMessage.Body = htmlBody;
                mailMessage.IsBodyHtml = true;

                // Đính kèm file
                if (attachmentPaths != null)
                {
                    foreach (var path in attachmentPaths)
                    {
                        if (File.Exists(path))
                        {
                            var attachment = new Attachment(path);
                            mailMessage.Attachments.Add(attachment);
                        }
                    }
                }

                using var client = new SmtpClient(settings["SmtpServer"], int.Parse(settings["SmtpPort"]));
                client.EnableSsl = bool.Parse(settings["EnableSsl"]);
                client.Credentials = new NetworkCredential(settings["Username"], settings["Password"]);

                // Quan trọng: .NET 7+ mới hỗ trợ async thật sự
                await client.SendMailAsync(mailMessage);

                _logger.Info($"[Microsoft SmtpClient] Gửi email THÀNH CÔNG tới: {string.Join(", ", toEmails)} | Subject: {subject}");
            }
            catch (Exception ex)
            {
                _logger.Error($"[Microsoft SmtpClient] Gửi email THẤT BẠI tới: {string.Join(", ", toEmails)}", ex);
                _logger.LogException(ex, $"Subject: {subject} | To: {string.Join(", ", toEmails)}");
            }
        }
    }
}