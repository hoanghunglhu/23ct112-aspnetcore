using LearnAspNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnAspNetCore.DbContext
{
    // SỬA DÒNG NÀY:
    // Thay vì ": DbContext"
    // Hãy viết rõ là ": Microsoft.EntityFrameworkCore.DbContext"
    public class NewsDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        // Dòng này bây giờ sẽ hợp lệ
        public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options)
        {
        }

        // Tên bảng. (Dựa trên hình ảnh của bạn, model là New.cs)
        public DbSet<New> News { get; set; }
    }
}