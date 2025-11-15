using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using log4net;
using System.Reflection;

namespace LearnApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        [HttpPost("send")]
        public IActionResult SendEmail(string to, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("vuduythai69@gmail.com", "mcmpienqrcezleiu"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("vuduythai69@gmail.com", "Hệ thống backend"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(to);

                smtpClient.Send(mailMessage);

                log.Info($"Đã gửi email tới {to} với tiêu đề: {subject}");
                return Ok("Gửi email thành công!");
            }
            catch (Exception ex)
            {
                log.Error("Gửi email thất bại", ex);
                return StatusCode(500, $"Lỗi gửi email: {ex.Message}");
            }
        }
    }
}
