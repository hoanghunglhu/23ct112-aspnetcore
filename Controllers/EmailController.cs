using Microsoft.AspNetCore.Mvc;
using LearnApiNetCore.Services;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("email/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _email;

        public EmailController(IEmailService email)
        {
            _email = email;
        }

        [HttpGet("/send-test")]
        public async Task<IActionResult> SendTest()
        {
            var html = "<h1>Đây là email test của mình nè. Hahaha</h1>";

            await _email.SendEmailAsync(
                toEmail: "vothingoc.hong1104+LearnASPDotNet@gmail.com",
                subject: "Email môn Lập trình Backend",
                htmlBody: html,
                attachmentPaths: new List<string> { "wwwroot/files/test.pdf" } // nếu có
            );

            return Content("ĐÃ GỬI EMAIL! Kiểm tra hộp thư và log.");
        }
    }
}