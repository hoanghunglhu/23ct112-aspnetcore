using Microsoft.EntityFrameworkCore;
using LearnApiNetCore.Entity;
using NLog.Web;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

try
{
    logger.Info("Starting application...");

    var builder = WebApplication.CreateBuilder(args);
    builder.Environment.WebRootPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");


    // Logging
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // DbContext
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Controllers + Swagger
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // 🔥 Bật dùng file tĩnh (HTML, CSS, JS)
    app.UseDefaultFiles(); 
 // dùng index.html mặc định
    app.UseStaticFiles();   // cho phép truy cập wwwroot

    // Swagger
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
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
