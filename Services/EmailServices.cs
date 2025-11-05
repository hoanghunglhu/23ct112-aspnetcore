using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _config;
    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public void SendEmail(string to, string subject, string body)
    {
        var settings = _config.GetSection("EmailSettings");
        var mail = new MailMessage()
        {
            From = new MailAddress(settings["Email"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mail.To.Add(to);

        using (var smtp = new SmtpClient(settings["SmtpServer"]))
        {
            smtp.Port = int.Parse(settings["Port"]);
            smtp.EnableSsl = bool.Parse(settings["EnableSsl"]);
            smtp.Credentials = new NetworkCredential(settings["Email"], settings["Password"]);
            smtp.Send(mail);
        }
    }
}
