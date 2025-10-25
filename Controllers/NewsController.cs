using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<NewsController> _logger;
        private const string NewsCacheKey = "news_list";
        
        // Danh sách tin tức trong memory (thay thế database)
        private static List<News> _newsStorage = new List<News>
        {
            new News
            {
                Id = 1,
                Title = "hieu1",
                Content = "agsad",
                Author = "hieu1",
                PublishedDate = DateTime.Now.AddDays(-1)
            },
            new News
            {
                Id = 2,
                Title = "hieu2",
                Content = "cbs",
                Author = "hieu2",
                PublishedDate = DateTime.Now.AddHours(-5)
            },
            new News
            {
                Id = 3,
                Title = "hieu3",
                Content = "abc",
                Author = "hieu3",
                PublishedDate = DateTime.Now.AddHours(-2)
            }
        };

        public NewsController(IMemoryCache memoryCache, ILogger<NewsController> logger)
        {
            _cache = memoryCache;
            _logger = logger;
        }

        // GET: api/news
        [HttpGet]
        public IActionResult GetNews()
        {
            bool isFromCache = false;
            
            // Kiểm tra xem dữ liệu có trong cache không
            if (!_cache.TryGetValue(NewsCacheKey, out List<News> newsList))
            {
                _logger.LogInformation("Cache MISS - Đang lấy dữ liệu từ database...");
                
                // Nếu không có trong cache, lấy dữ liệu mới
                newsList = GetNewsFromDatabase();

                // Cấu hình cache options
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(60)); // Cache trong 60 giây

                // Lưu vào cache
                _cache.Set(NewsCacheKey, newsList, cacheOptions);
                
                _logger.LogInformation("Đã lưu dữ liệu vào cache với thời gian 60 giây");
                isFromCache = false;
            }
            else
            {
                _logger.LogInformation("Cache HIT - Đang lấy dữ liệu từ cache");
                isFromCache = true;
            }

            return Ok(new
            {
                success = true,
                data = newsList,
                isFromCache = isFromCache,
                timestamp = DateTime.Now,
                message = isFromCache ? "Dữ liệu từ cache" : "Dữ liệu từ database"
            });
        }

        // POST: api/news/clear-cache
        [HttpPost("clear-cache")]
        public IActionResult ClearCache()
        {
            _cache.Remove(NewsCacheKey);
            _logger.LogInformation("Cache đã được xóa thủ công");

            return Ok(new
            {
                success = true,
                message = "Cache đã được xóa thành công",
                timestamp = DateTime.Now
            });
        }

        // POST: api/news
        [HttpPost]
        public IActionResult AddNews([FromBody] CreateNewsRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest(new { success = false, message = "Title không được để trống" });
            }

            var newNews = new News
            {
                Id = _newsStorage.Count > 0 ? _newsStorage.Max(n => n.Id) + 1 : 1,
                Title = request.Title,
                Content = request.Content ?? "",
                Author = request.Author ?? "Anonymous",
                PublishedDate = DateTime.Now
            };

            _newsStorage.Add(newNews);
            
            // KHÔNG xóa cache - dữ liệu mới sẽ hiển thị sau khi cache hết hạn (60 giây)
            
            _logger.LogInformation($"Đã thêm tin tức mới với ID: {newNews.Id}. Dữ liệu sẽ hiển thị sau khi cache hết hạn.");

            return Ok(new
            {
                success = true,
                message = "Thêm tin tức thành công. Dữ liệu mới sẽ hiển thị sau 60 giây (khi cache hết hạn)",
                data = newNews
            });
        }

        
        // Phương thức giả lập lấy dữ liệu từ database
        private List<News> GetNewsFromDatabase()
        {
            // Trả về dữ liệu từ memory storage
            _logger.LogInformation($"Đang lấy {_newsStorage.Count} tin tức từ storage");
            return _newsStorage.ToList();
        }
    }

    // Model News
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
    }

    // Request model để thêm tin tức mới
    public class CreateNewsRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
    }
}