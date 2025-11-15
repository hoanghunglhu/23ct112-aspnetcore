

using Microsoft.EntityFrameworkCore;

using LearnApiNetCore.Entity;

using log4net;
using log4net.Config;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// ====== CẤU HÌNH LOG4NET ======
var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
var log4netFile = new FileInfo("log4net.config");
XmlConfigurator.Configure(logRepository, log4netFile);

// Add DbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<EmailService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

<<<<<<< Updated upstream
=======
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
>>>>>>> Stashed changes
app.MapControllers();


app.Run();

