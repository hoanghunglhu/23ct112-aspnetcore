
using Microsoft.EntityFrameworkCore;
using LearnApiNetCore.Entity;
using log4net;
using log4net.Config;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);

var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
var log4netConfigPath = Path.Combine(AppContext.BaseDirectory, "log4net.config");
XmlConfigurator.Configure(logRepository, new FileInfo(log4netConfigPath));

var log = LogManager.GetLogger(typeof(Program));


builder.Services.AddMemoryCache();
// Add DbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

log.Info("Ứng dụng Web API đang khởi động...");

if(app.Environment.IsDevelopment())
{
     app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();

try
{
    log.Info("Ứng dụng bắt đầu chạy...");
    app.Run();
}
catch (Exception ex)
{
    log.Error("Ứng dụng bị lỗi nghiêm trọng:", ex);
}
finally
{
    log.Info("Ứng dụng đã dừng.");
}


