using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;

namespace LearnApiNetCore.Services
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailBackgroundService> _logger;
        private static readonly ConcurrentQueue<(string To, string Subject, string Body)> _queue = new();

        public EmailBackgroundService(IEmailService emailService, ILogger<EmailBackgroundService> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public static void EnqueueEmail(string to, string subject, string body)
        {
            _queue.Enqueue((to, subject, body));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("📬 Email background service đang chạy...");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_queue.TryDequeue(out var email))
                {
                    try
                    {
                        await _emailService.SendEmailAsync(email.To, email.Subject, email.Body);
                        _logger.LogInformation($" Đã gửi email tới {email.To}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, " Lỗi khi gửi email");
                    }
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}