using Microsoft.EntityFrameworkCore;
using LearnApiNetCore.Entity;
using NLog;
using NLog.Web;
using LearnApiNetCore.Models;
using LearnApiNetCore.Services;

var logger = LogManager.Setup()
    .LoadConfigurationFromFile("NLog.config")
    .GetCurrentClassLogger();

try
{
    logger.Info("Khởi động ứng dụng...");

    var builder = WebApplication.CreateBuilder(args);

    // Thêm NLog vào hệ thống logging mặc định
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add DbContext with SQL Server
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddMemoryCache();
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();

    // Đọc cấu hình SMTP từ appsettings.json
    builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

    // Đăng ký EmailService để inject vào Controller
    builder.Services.AddScoped<EmailService>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    // Ghi lại lỗi khởi động ứng dụng (nếu có)
    logger.Error(ex, "Ứng dụng gặp lỗi nghiêm trọng khi khởi động!");
    throw;
}
finally
{
    LogManager.Shutdown();
}
