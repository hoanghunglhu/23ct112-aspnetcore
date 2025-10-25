using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "news_list";

        // Biến static để lưu trạng thái đã tạo cache lần đầu chưa
        private static bool isFirstTime = true;

        public NewsController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public IActionResult GetNews()
        {
            if (!_cache.TryGetValue(CacheKey, out List<string> news))
            {
                if (isFirstTime)
                {
                    // Lần đầu tạo 3 tin
                    news = new List<string>
                    {
                        $"Bản tin 1: ASP.NET Core ngày càng phổ biến - {DateTime.Now:T}",
                        $"Bản tin 2: Microsoft ra mắt .NET 9 - {DateTime.Now:T}",
                        $"Bản tin 3: AI hỗ trợ lập trình hiệu quả hơn - {DateTime.Now:T}"
                    };
                    isFirstTime = false;
                }
                else
                {
                    // Lần sau (cache hết) tạo 4 tin
                    news = new List<string>
                    {
                        $"Bản tin 1: ASP.NET Core ngày càng phổ biến - {DateTime.Now:T}",
                        $"Bản tin 2: Microsoft ra mắt .NET 9 - {DateTime.Now:T}",
                        $"Bản tin 3: AI hỗ trợ lập trình hiệu quả hơn - {DateTime.Now:T}",
                        $"Bản tin 4: OpenAI phát hành GPT-5 hỗ trợ lập trình viên - {DateTime.Now:T}"
                    };
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));

                _cache.Set(CacheKey, news, cacheOptions);

                return Ok(new
                {
                    source = "database",
                    data = news
                });
            }

            return Ok(new
            {
                source = "cache",
                data = news
            });
        }

        [HttpPost("clear-cache")]
        public IActionResult ClearCache()
        {
            _cache.Remove(CacheKey);
            return Ok(new { message = "Đã xóa cache thành công!" });
        }
    }
}
