using Microsoft.AspNetCore.Mvc;
using LearnApiNetCore.Services;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromQuery] string to)
        {
            string subject = "Thử nghiệm gửi mail từ ASP.NET Core";
            string body = "<h3>Xin chào!</h3><p>Đây là email test từ Viên Xuân Quý 😊</p>";

            bool result = await _emailService.SendEmailAsync(to, subject, body);

            if (result)
                return Ok("Đã gửi email thành công!");
            else
                return StatusCode(500, "Gửi email thất bại!");
        }
    }
}
