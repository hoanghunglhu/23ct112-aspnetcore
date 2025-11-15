using LearnApiNetCore.Entity;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

using EmailDemo.Services;

var logger = LogManager.Setup()
    .LoadConfigurationFromFile("Nlog.config") 
    .GetCurrentClassLogger();

try
{
    logger.Info("Ứng dụng đang khởi động...");

    var builder = WebApplication.CreateBuilder(args);

    // Cấu hình NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Cấu hình DbContext
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    //Thêm EmailService
    builder.Services.AddSingleton<EmailService>();

    // Cấu hình controller và Swagger
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Cấu hình pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseStaticFiles();
    app.UseDefaultFiles();

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    logger.Info("Ứng dụng đã khởi động thành công.");
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Lỗi khi khởi động ứng dụng.");
    throw;
}
finally
{
    LogManager.Shutdown();
}
