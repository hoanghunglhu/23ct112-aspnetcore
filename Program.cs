using LearnApiNetCore.Entity;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    
    builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
    builder.Services.AddHostedService<EmailBackgroundService>();

    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();
    builder.Services.AddMemoryCache();

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
    logger.Error(ex, "Unhandled exception tại Program.cs");
    EmailBackgroundService.Enqueue(ex);
    throw;
}
finally
{
    LogManager.Shutdown();
}
