using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "newsList";

        public NewsController(IMemoryCache cache)
        {
            _cache = cache;
        }
        [HttpGet]
        public IActionResult GetNews()
        {
            if (!_cache.TryGetValue(CacheKey, out List<object> newsList))
            {
                newsList = new List<object>
                {
                    new { Id = 1, Title = "Tin mới: Công nghệ AI bùng nổ", Date = DateTime.Now },
                    new { Id = 2, Title = "LHU đạt thành tích cao trong nghiên cứu", Date = DateTime.Now },
                    new { Id = 3, Title = "Sinh viên CNTT giành giải Hackathon 2025", Date = DateTime.Now },
                    new { Id = 4, Title = "Lễ tổng kết năm học 2024-2025 khoa CNTT ", Date = DateTime.Now }
                };

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));

                _cache.Set(CacheKey, newsList, cacheOptions);

                return Ok(new
                {
                    message = "Dữ liệu được tạo mới (chưa có trong cache)",
                    data = newsList
                });
            }
            return Ok(new
            {
                message = "Dữ liệu lấy từ cache",
                data = newsList
            });
        }
        [HttpPost("clear-cache")]
        public IActionResult ClearCache()
        {
            _cache.Remove(CacheKey);
            return Ok(new { message = "Cache đã được xóa thành công!" });
        }
    }
}