using LearnAspNetCore.DbContext; // <-- THÊM USING NÀY
using Microsoft.EntityFrameworkCore; // <-- THÊM USING NÀY

var builder = WebApplication.CreateBuilder(args);

// === THÊM DỊCH VỤ ===
builder.Services.AddControllers();

// 1. Đăng ký IMemoryCache
builder.Services.AddMemoryCache();

// 2. Đăng ký DbContext
builder.Services.AddDbContext<NewsDbContext>(options =>
{
    // Đọc chuỗi kết nối từ appsettings.json
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// === CẤU HÌNH PIPELINE ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();