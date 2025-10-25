using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("news")]
    public class NewsController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "NewsList";
        private static List<string> _allNews = new List<string>
        {
            "Tin 1: Hôm nay trời đẹp",
            "Tin 2: Giá xăng giảm",
            "Tin 3: AI phát triển mạnh"
        };

        public NewsController(IMemoryCache cache)
        {
            _cache = cache;
        }

        // ✅ Lấy danh sách tin tức (cache 60 giây)
        [HttpGet]
        public IActionResult GetNews()
        {
            if (!_cache.TryGetValue(CacheKey, out List<string> cachedNews))
            {
                // Cache hết hạn hoặc chưa có → tạo mới
                cachedNews = new List<string>(_allNews);

                // Log ra console để dễ quan sát
                Console.WriteLine($"[CACHE] ➕ Tạo cache mới lúc {DateTime.Now:HH:mm:ss}");

                // Thiết lập cache 60 giây
                _cache.Set(CacheKey, cachedNews, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60),
                    PostEvictionCallbacks =
                    {
                        new PostEvictionCallbackRegistration
                        {
                            EvictionCallback = (key, value, reason, state) =>
                            {
                                Console.WriteLine($"[CACHE] ❌ Cache '{key}' bị xóa lúc {DateTime.Now:HH:mm:ss} (Lý do: {reason})");
                            }
                        }
                    }
                });
            }

            Console.WriteLine($"[CACHE] 📤 Trả dữ liệu lúc {DateTime.Now:HH:mm:ss}");

            return Ok(new
            {
                Time = DateTime.Now.ToString("HH:mm:ss"),
                Data = cachedNews
            });
        }

        // ✅ Thêm tin mới (chưa cập nhật cache)
        [HttpPost]
        public IActionResult AddNews([FromBody] string newNews)
        {
            _allNews.Add(newNews);
            Console.WriteLine($"[DATA] ➕ Thêm tin mới: '{newNews}' lúc {DateTime.Now:HH:mm:ss}");
            return Ok(new { message = "Đã thêm tin mới", total = _allNews.Count });
        }

        // ✅ Xóa cache thủ công
        [HttpPost("clear-cache")]
        public IActionResult ClearCache()
        {
            if (_cache.TryGetValue(CacheKey, out _))
            {
                _cache.Remove(CacheKey);
                Console.WriteLine($"[CACHE] 🧹 Xóa cache thủ công lúc {DateTime.Now:HH:mm:ss}");
                return Ok(new { message = "Đã xóa cache hiện tại" });
            }
            else
            {
                Console.WriteLine($"[CACHE] ⚠️ Không có cache để xóa (cache đã hết hạn) - {DateTime.Now:HH:mm:ss}");
                return Ok(new { message = "Cache đã hết hạn hoặc chưa được tạo" });
            }
        }
    }
}
