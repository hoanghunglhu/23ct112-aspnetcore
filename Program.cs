using Microsoft.EntityFrameworkCore;
using LearnApiNetCore.Entity;
using log4net;
using log4net.Config;
using System.Reflection;
using Log4NetManager = log4net.LogManager;
using NLogManager = NLog.LogManager;
using NLogBuilder = NLog.Web.NLogBuilder;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình log4net
var logRepository = Log4NetManager.GetRepository(Assembly.GetEntryAssembly());
var log4netConfigPath = Path.Combine(AppContext.BaseDirectory, "log4net.config");
XmlConfigurator.Configure(logRepository, new FileInfo(log4netConfigPath));
var log4 = Log4NetManager.GetLogger(typeof(Program));

// Cấu hình NLog
var nlogger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

// Cấu hình dịch vụ
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllersWithViews();  // Đảm bảo sử dụng MVC
builder.Services.AddSwaggerGen();  // Chỉ cần nếu bạn sử dụng Swagger
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<EmailService>();

// Xây dựng ứng dụng
var app = builder.Build();

log4.Info("Ứng dụng Web API đang khởi động...");
nlogger.Info("Ứng dụng Web API đang khởi động...");

// Cấu hình môi trường Development (Swagger)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Cấu hình Static Files và HTTPS
app.UseStaticFiles();
app.UseDefaultFiles();  // Cho phép sử dụng index.html, login.html trong thư mục wwwroot

app.UseHttpsRedirection();

// Cấu hình các Route cho controller
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

// Thêm Controller cho đăng nhập
app.MapControllers();

// Log thông tin khi ứng dụng bắt đầu
try
{
    log4.Info("Ứng dụng bắt đầu chạy...");
    nlogger.Info("Ứng dụng bắt đầu chạy...");
    app.Run();  // Chạy ứng dụng
}
catch (Exception ex)
{
    // Log lỗi khi có exception
    log4.Error("Ứng dụng bị lỗi nghiêm trọng:", ex);
    nlogger.Error(ex, "Ứng dụng bị lỗi nghiêm trọng:");
}
finally
{
    // Log khi ứng dụng dừng
    log4.Info("Ứng dụng đã dừng.");
    nlogger.Info("Ứng dụng đã dừng.");
    NLogManager.Shutdown(); // Đảm bảo NLog dừng một cách an toàn
}
