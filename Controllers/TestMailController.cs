using Microsoft.AspNetCore.Mvc;
using LearnApiNetCore.Services;

namespace LearnApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestMailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public TestMailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("send")]
        public IActionResult SendEmail()
        {
        var result = _emailService.SendEmail(
                "truongthianh23ct112@gmail.com",
                "Test Email",
                "<h3> Gửi email thành công từ ASP.NET Core!</h3>"
            );

            return Ok(result ? " Thành công" : " Thất bại");
        }
    }
}
