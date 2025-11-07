using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace EmailDemo.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            var smtpSettings = _config.GetSection("Smtp"); 

            string senderEmail = smtpSettings["Username"];
            string password = smtpSettings["Password"];

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Hệ thống", senderEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using (var client = new SmtpClient())
            {
                client.Connect(
                    smtpSettings["Host"],
                    int.Parse(smtpSettings["Port"]),
                    MailKit.Security.SecureSocketOptions.StartTls
                );
                client.Authenticate(senderEmail, password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
