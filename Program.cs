using Microsoft.EntityFrameworkCore;
using LearnApiNetCore.Entity;
using LearnApiNetCore.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Cấu hình Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // dùng Serilog thay cho logging mặc định

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<LearnApiNetCore.Services.EmailService>();

var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
var log4netFile = new FileInfo("log4net.config");
XmlConfigurator.Configure(logRepository, log4netFile);

// Add DbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Middleware xử lý lỗi toàn cục
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        Log.Error(exception, "Unhandled exception occurred!");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An unexpected error occurred!");
    });
});


if(app.Environment.IsDevelopment())
{
     app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

