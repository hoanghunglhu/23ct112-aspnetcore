
using LearnApiNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemController : ControllerBase
    {
        private readonly EmailService _emailService;
        private readonly ILogger<SystemController> _logger;

        public SystemController(EmailService emailService, ILogger<SystemController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }
        [HttpGet("test-log")]
        public IActionResult TestLogging()
        {
            try
            {
                _logger.LogInformation("Bắt đầu test log lỗi");
                throw new Exception("Đây là lỗi giả lập để test logging");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi trong TestLogging()");
                return StatusCode(500, "Đã ghi log lỗi vào file Logs");
            }
        }
        [HttpPost("send-mail")]
        public IActionResult SendMail([FromBody] EmailRequest request)
        {
            try
            {
                _emailService.SendEmail(
                    request.To,
                    request.Subject,
                    request.Body
                );

                _logger.LogInformation("Đã gửi mail đến {To}", request.To);
                return Ok($"Đã gửi mail thành công đến {request.To}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gửi mail đến {To}", request.To);
                return StatusCode(500, "Gửi mail thất bại");
            }
        }
    }
    public class EmailRequest
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = "Test Email";
        public string Body { get; set; } = "Xin chào, đây là nội dung mặc định!";
    }
}


