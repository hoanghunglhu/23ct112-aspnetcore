
using Microsoft.EntityFrameworkCore;
using LearnApiNetCore.Entity;
using log4net;
using log4net.Config;
using System.Reflection;
using Log4NetManager = log4net.LogManager;
using NLogManager = NLog.LogManager;
using NLogBuilder = NLog.Web.NLogBuilder;

var builder = WebApplication.CreateBuilder(args);

var logRepository = Log4NetManager.GetRepository(Assembly.GetEntryAssembly());
var log4netConfigPath = Path.Combine(AppContext.BaseDirectory, "log4net.config");
XmlConfigurator.Configure(logRepository, new FileInfo(log4netConfigPath));
var log4 = Log4NetManager.GetLogger(typeof(Program));

var nlogger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

builder.Services.AddMemoryCache();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

log4.Info("Ứng dụng Web API đang khởi động...");
nlogger.Info("Ứng dụng Web API đang khởi động...");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

try
{
    log4.Info("Ứng dụng bắt đầu chạy...");
    nlogger.Info("Ứng dụng bắt đầu chạy...");
    app.Run();
}
catch (Exception ex)
{
    // Ghi lỗi bằng cả 2 logger
    log4.Error("Ứng dụng bị lỗi nghiêm trọng:", ex);
    nlogger.Error(ex, "Ứng dụng bị lỗi nghiêm trọng:");
}
finally
{
    log4.Info("Ứng dụng đã dừng.");
    nlogger.Info("Ứng dụng đã dừng.");
    NLogManager.Shutdown(); // Dừng NLog an toàn
}


