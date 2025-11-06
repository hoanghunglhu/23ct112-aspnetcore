using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MailController : ControllerBase
{
    private readonly EmailService _emailService;

    public MailController(EmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMail(string toEmail)
    {
        try
        {
            await _emailService.SendEmailAsync(
                toEmail,
                "Thông báo từ hệ thống Boba-Tea",
                "<h3>Xin chào bạn!</h3><p>Cảm ơn bạn đã đặt hàng tại Yêu Nước Boba-Tea ❤️</p>"
            );

            return Ok("Gửi email thành công!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Gửi mail thất bại: {ex.Message}");
        }
    }
}
