using Microsoft.AspNetCore.Mvc;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        // Inject EmailService vào controller
        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        // Gửi email test đơn giản
        [HttpGet("send")]
        public IActionResult SendTestEmail()
        {
            string to = "fw62262@gmail.com"; // email người nhận
            string subject = "Test gửi email từ ASP.NET Core";
            string body = "Xin chào! Đây là email test thành công.";

            _emailService.SendEmail(to, subject, body);

            return Ok("Đã gửi email thành công!");
        }

        // Gửi email có nội dung từ client
        [HttpPost("send")]
        public IActionResult SendEmailDynamic([FromBody] EmailRequest request)
        {
            _emailService.SendEmail(request.To, request.Subject, request.Body);
            return Ok("Email đã được gửi!");
        }
    }

    // Model nhận dữ liệu từ body
    public class EmailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
