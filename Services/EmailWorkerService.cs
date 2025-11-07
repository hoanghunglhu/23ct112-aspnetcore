using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LearnApiNetCore.Services; 
namespace LearnApiNetCore.Services;

public class EmailWorkerService : BackgroundService
{
    private readonly ILogger<EmailWorkerService> _logger;
    private readonly IEmailQueue _emailQueue;
    private readonly IEmailService _emailService; // Dịch vụ MailKit bạn đã tạo

    public EmailWorkerService(
        ILogger<EmailWorkerService> logger,
        IEmailQueue emailQueue,
        IEmailService emailService) 
    {
        _logger = logger;
        _emailQueue = emailQueue;
        _emailService = emailService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Email Worker Service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            if (_emailQueue.TryDequeue(out EmailMessage message))
            {
                try
                {
                    // Gửi email bằng dịch vụ MailKit
                    await _emailService.SendEmailAsync(message);
                    _logger.LogInformation("Email sent successfully to {ToEmail}", message.ToEmail);
                }
                catch (Exception ex)
                {
                    // Ghi log ngoại lệ (dùng phương thức bạn đã tạo ở bài trước)
                    _logger.LogError(ex, "Failed to send email to {ToEmail}", message.ToEmail);
                    // Tùy chọn: Thêm lại vào queue hoặc lưu vào CSDL để thử lại sau
                }
            }

            // Chờ 5 giây trước khi kiểm tra queue lần tiếp theo
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
        _logger.LogInformation("Email Worker Service is stopping.");
    }
}