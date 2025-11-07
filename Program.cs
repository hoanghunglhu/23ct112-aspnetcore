
using Microsoft.EntityFrameworkCore;
using LearnApiNetCore.Entity;
using LearnApiNetCore.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using log4net;
using log4net.Config;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found")));

// Register JWT Service
// builder.Services.AddScoped<IJwtService, JwtService>();

// Configure JWT Authentication
// var jwtSettings = builder.Configuration.GetSection("JwtSettings");
// var secretKey = jwtSettings["SecretKey"];

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuerSigningKey = true,
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey!)),
//         ValidateIssuer = true,
//         ValidIssuer = jwtSettings["Issuer"],
//         ValidateAudience = true,
//         ValidAudience = jwtSettings["Audience"],
//         ValidateLifetime = true,
//         ClockSkew = TimeSpan.Zero
//     };
// });

// builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ILog>(LogManager.GetLogger(typeof(Logger)));
builder.Services.AddSingleton<ILogService, Logger>();

var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepo, new FileInfo("log4net.config"));

builder.Services.Configure<IEmailService>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton<IEmailService, EmailService>();

var app = builder.Build();

var log = LogManager.GetLogger(typeof(Program));
log.Info("=== Ứng dụng khởi động ===");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();
