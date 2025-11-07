using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using EmailDemo.Services; 

namespace EmailDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public MailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public IActionResult SendEmail([FromBody] EmailRequest request)
        {
            try
            {
                _emailService.SendEmail(request.ToEmail, request.Subject, request.Body);
                return Ok(new { message = "Email đã được gửi thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Lỗi khi gửi email: {ex.Message}" });
            }
        }
    }

    
    public class EmailRequest
    {
        public string ToEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
