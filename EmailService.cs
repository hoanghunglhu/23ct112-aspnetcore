using MailKit.Net.Smtp;
using MimeKit;

namespace LearnAspNetCore.Services
{
    public class EmailBackgroundService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                // Có thể gọi gửi email tự động ở đây nếu muốn
            }
        }
    }
}
