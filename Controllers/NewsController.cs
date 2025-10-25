using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;


namespace LearnApiNetCore.Controllers
{
  [ApiController]
  [Route("controller")]

  public class NewsController : ControllerBase
  {
    private readonly IMemoryCache _cache;
    private const string CACHE_NAME = "newsData";

    public NewsController(IMemoryCache cache)
    {
        _cache = cache;
    }
    public class News
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public DateTime Date { get; set; }
        }

    // GET /news
    [HttpGet]
    public IActionResult GetNews()
    {
        if (!_cache.TryGetValue(CACHE_NAME, out List<News> newsList))
        {
            newsList = new List<News>
            {
                new News { Id = 1, Title = "Khám phá vũ trụ mới", Date = DateTime.Now },
                new News { Id = 2, Title = "Công nghệ pin tiên tiến", Date = DateTime.Now },
                new News { Id = 3, Title = "Xe điện phổ biến", Date = DateTime.Now },
                new News { Id = 4, Title = "LHU-Dạy thật, Học thật", Date = DateTime.Now }
            };
            _cache.Set(CACHE_NAME, newsList, TimeSpan.FromSeconds(60));
        }
        return Ok(newsList);
    }

    // POST /news/clear-cache
    [HttpPost("clear-cache")]
    public IActionResult ClearCache()
    {
        _cache.Remove(CACHE_NAME);
        return Ok("Cache đã được xóa!");
    }
  }
}