using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace _23ct112_aspnetcore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        [HttpPost("send")]
        public IActionResult SendEmail(string toEmail)
        {
            try
            {
                string fromEmail = "leminhluan087@gmail.com";        
                string appPassword = "mnde wqig jxcl wmwq";
                
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail, "Hệ thống ASP.NET Demo");
                mail.To.Add(toEmail);
                mail.Subject = "Thử gửi email từ ASP.NET Web API";
                mail.Body = "Xin chào, đây là email test gửi qua Swagger!";
                mail.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential(fromEmail, appPassword);
                smtp.EnableSsl = true;

                smtp.Send(mail);

                return Ok("✅ Gửi email thành công!");
            }
            catch (Exception ex)
            {
                return BadRequest("❌ Lỗi khi gửi email: " + ex.Message);
            }
        }
    }
}