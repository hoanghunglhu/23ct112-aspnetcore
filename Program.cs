using Microsoft.EntityFrameworkCore;
using LearnApiNetCore.Entity;
using log4net;
using log4net.Config;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<EmailService>();

// ====== CẤU HÌNH LOG4NET ======
var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
var log4netFilePath = Path.Combine(AppContext.BaseDirectory, "log4net.config");
var log4netFile = new FileInfo(log4netFilePath);

// Kiểm tra file config có tồn tại
if (!log4netFile.Exists)
{
    throw new FileNotFoundException("log4net.config không tìm thấy tại: " + log4netFile.FullName);
}

// Tạo folder Logs nếu chưa có
var logsFolder = Path.Combine(AppContext.BaseDirectory, "Logs");
if (!Directory.Exists(logsFolder))
{
    Directory.CreateDirectory(logsFolder);
}

// Khởi tạo log4net
XmlConfigurator.Configure(logRepository, log4netFile);

// Test log ngay startup
var startupLogger = LogManager.GetLogger(typeof(Program));
startupLogger.Info("Log4net khởi tạo thành công và folder Logs đã sẵn sàng.");

// ====== CẤU HÌNH DỊCH VỤ ======
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();


var app = builder.Build();

// ====== MIDDLEWARE SWAGGER ======
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// ====== KHỞI ĐỘNG ỨNG DỤNG ======
app.Run();
