using LearnApiNetCore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "news_list";

        public NewsController(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        //  GET /api/news
        [HttpGet]
        public IActionResult GetAll()
        {
            if (!_cache.TryGetValue(CacheKey, out List<User> newsList))
            {
                Console.WriteLine("📥 Cache trống → Lấy dữ liệu từ DB");

                newsList = _context.Users.ToList();

                // Lưu cache trong 60 giây
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));

                _cache.Set(CacheKey, newsList, cacheOptions);
            }
            else
            {
                Console.WriteLine("Lấy dữ liệu từ cache (không truy vấn DB)");
            }

            return Ok(newsList);
        }

        //  POST /api/news/clear-cache
        [HttpPost("clear-cache")]
        public IActionResult ClearCache()
        {
            _cache.Remove(CacheKey);
            Console.WriteLine("🧹 Cache đã được xóa thủ công");
            return Ok(new { message = "Cache cleared successfully!" });
        }
    }
}