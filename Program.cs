using Microsoft.EntityFrameworkCore;
using LearnApiNetCore.Entity;
using LearnApiNetCore.Services;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// ✅ Logging cấu hình NGAY SAU builder
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Add services
builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<EmailService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
