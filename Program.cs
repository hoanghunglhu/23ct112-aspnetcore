
using Microsoft.EntityFrameworkCore;

using LearnApiNetCore.Entity;
using NLog;
using NLog.Web;
using LearnApiNetCore.Services;

var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
logger.Info("applicatio starting....");
try
{
    var builder = WebApplication.CreateBuilder(args);



    // Add DbContext with SQL Server
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSetting"));
    builder.Services.AddScoped<IEmailService, EmailService>();
   
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseStaticFiles();
    app.UseDefaultFiles();
    
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "stop program bc of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}
