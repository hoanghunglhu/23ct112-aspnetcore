using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;

namespace LearnAspNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Demo System", "your_email@gmail.com"));
            message.To.Add(new MailboxAddress("", "recipient_email@gmail.com"));
            message.Subject = "Test gửi email từ ASP.NET Core";
            message.Body = new TextPart("plain")
            {
                Text = "Xin chào, đây là email demo gửi bằng MailKit!"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync("your_email@gmail.com", "app_password");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return Ok("Email sent successfully!");
        }
    }
}
