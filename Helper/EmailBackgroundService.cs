using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Mail;

public class EmailBackgroundService : BackgroundService
{
    private readonly ILogger<EmailBackgroundService> _logger;
    private readonly SmtpSettings _smtp;
    private static readonly ConcurrentQueue<Exception> _queue = new();

    public EmailBackgroundService(ILogger<EmailBackgroundService> logger, IOptions<SmtpSettings> smtp)
    {
        _logger = logger;
        _smtp = smtp.Value;
    }

    public static void Enqueue(Exception ex)
    {
        _queue.Enqueue(ex);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Email background service đang chạy...");
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_queue.TryDequeue(out var ex))
            {
                try
                {
                    await SendEmailAsync(ex);
                }
                catch (Exception mailEx)
                {
                    _logger.LogError(mailEx, "Không thể gửi email lỗi");
                }
            }

            await Task.Delay(5000, stoppingToken);
        }
    }

    private async Task SendEmailAsync(Exception ex)
    {
        using var message = new MailMessage
        {
            From = new MailAddress(_smtp.User),
            Subject = "Lỗi hệ thống API",
            Body = $"Thời gian: {DateTime.Now}\n\n{ex}",
        };
        message.To.Add(_smtp.To);

        using var client = new SmtpClient(_smtp.Host, _smtp.Port)
        {
            Credentials = new NetworkCredential(_smtp.User, _smtp.Password),
            EnableSsl = _smtp.EnableSsl
        };

        await client.SendMailAsync(message);
        _logger.LogInformation("Đã gửi email thông báo lỗi đến {to}", _smtp.To);
    }
}
