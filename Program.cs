using Microsoft.EntityFrameworkCore;
using LearnApiNetCore.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext nếu cần
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// Cho phép CORS nếu fetch từ domain khác (tùy chọn)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll"); // enable CORS
app.UseStaticFiles();    // phục vụ login.html + index.html
app.UseDefaultFiles();   // tự động load index.html nếu truy cập /
app.MapControllers();

app.Run();
