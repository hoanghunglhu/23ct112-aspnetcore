using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace LearnApiNetCore.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public bool SendEmail(string to, string subject, string body)
        {
            try
            {
                var settings = _config.GetSection("EmailSettings");

                var client = new SmtpClient(settings["SmtpServer"])
                {
                    Port = int.Parse(settings["Port"]),
                    EnableSsl = true,
                    Credentials = new NetworkCredential(
                        settings["Username"],
                        settings["Password"]
                    )
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(settings["SenderEmail"], settings["SenderName"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Email Error: " + ex.Message);
                return false;
            }
        }
    }
}
