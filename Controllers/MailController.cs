using LearnApiNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public MailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMail(string to, string subject, string body)
        {
            if (string.IsNullOrEmpty(to))
            {
                return BadRequest("Vui lòng nhập email người nhận.");
            }

            await _emailService.SendEmailAsync(to, subject, body);
            return Ok("Gửi mail thành công!");
        }
    }
}