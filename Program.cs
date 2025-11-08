using NLog.Web;
using Microsoft.EntityFrameworkCore;
using LearnApiNetCore.Entity;
using NLog; // Import thêm namespace này cho Logger

// Khởi tạo Logger TRƯỚC KHI tạo builder
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    // === PHẦN 1: KHỞI TẠO BUILDER VÀ CẤU HÌNH LOGGING ===
    var builder = WebApplication.CreateBuilder(args);
    
    // 1. Cấu hình NLog (theo hướng dẫn)
    builder.Logging.ClearProviders(); // Xóa các providers logging mặc định (Console, Debug...)
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog(); // Thêm NLog vào Host Builder

    // === PHẦN 2: CẤU HÌNH DỊCH VỤ (SERVICES) ===
    
    // Add DbContext with SQL Server (Dịch vụ của bạn)
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();

    // === PHẦN 3: XÂY DỰNG ỨNG DỤNG VÀ CẤU HÌNH MIDDLEWARE ===
    
    var app = builder.Build(); // CHỈ BUILD MỘT LẦN

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    // app.UseHttpsRedirection(); // Thường có nếu muốn dùng HTTPS

    app.MapControllers();

    // === PHẦN 4: CHẠY ỨNG DỤNG ===
    
    app.Run(); // Ứng dụng chạy tại đây
}
catch (Exception exception)
{
    // Ghi log khi xảy ra lỗi khởi động ứng dụng
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Đảm bảo flush log khi ứng dụng kết thúc
    LogManager.Shutdown(); // Thay NLog.LogManager.Shutdown() bằng LogManager.Shutdown() sau khi import NLog
}