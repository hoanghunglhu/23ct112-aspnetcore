using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "news_list";

        public NewsController(IMemoryCache cache)
        {
            _cache = cache;
        }

        // ✅ GET /news
        [HttpGet]
        public IActionResult GetNews()
        {
            // Kiểm tra cache có dữ liệu chưa
            if (!_cache.TryGetValue(CacheKey, out List<string> newsList))
            {
                // Lần đầu chỉ lưu 3 tin
                newsList = new List<string>
                {
                    "Tin 1: Bộ GD&ĐT công bố điểm chuẩn đại học 2025, nhiều ngành tăng mạnh.",
                    "Tin 2: Phạt 7,5 triệu đồng đối tượng đăng tải nội dung sai sự thật trên TikTok.",
                    "Tin 3: TikTok đồng hành cùng chiến dịch quảng bá di sản Việt Nam!"
                };

                // Cache sống 60 giây
                _cache.Set(CacheKey, newsList, new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(60)));

                Console.WriteLine("Lưu 3 tin đầu tiên vào cache.");
                return Ok(newsList);
            }

            // Cache hết hạn → tạo lại danh sách 4 tin
            newsList = new List<string>
            {
                "Tin 1: Bộ GD&ĐT công bố điểm chuẩn đại học 2025, nhiều ngành tăng mạnh.",
                "Tin 2: Phạt 7,5 triệu đồng đối tượng đăng tải nội dung sai sự thật trên TikTok.",
                "Tin 3: TikTok đồng hành cùng chiến dịch quảng bá di sản Việt Nam!",
                "Tin 4: TikTok Shop Vietnam Summit 2025: Tăng trưởng mạnh mẽ"
            };

            _cache.Set(CacheKey, newsList, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(60)));

            Console.WriteLine("Lưu 4 tin vào cache sau 60s.");
            return Ok(newsList);
        }

        // ✅ POST /news/clear-cache
        [HttpPost("clear-cache")]
        public IActionResult ClearCache()
        {
            _cache.Remove(CacheKey);
            Console.WriteLine("Cache đã được xóa.");
            return Ok(new { message = "Cache cleared successfully." });
        }
    }
}
