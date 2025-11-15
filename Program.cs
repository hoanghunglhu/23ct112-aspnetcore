using log4net;
using log4net.Config;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// --- Cấu hình log4net ---
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
var logger = LogManager.GetLogger(typeof(Program));

// --- Cấu hình mặc định ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

try
{
logger.Info("Ứng dụng đang khởi động...");
app.Run();
}
catch (Exception ex)
{
logger.Error("Lỗi nghiêm trọng khi khởi động ứng dụng.", ex);
throw;
}
finally
{
logger.Info("Ứng dụng đã dừng.");
}