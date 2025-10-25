using LearnApiNetCore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("news")] 
    public class NewsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public NewsController(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNews()
        {
            const string cacheKey = "newsList";

            if (!_cache.TryGetValue(cacheKey, out List<News> newsList))
            {
                newsList = await _context.News.ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(60)); 
                _cache.Set(cacheKey, newsList, cacheOptions);
            }

            return Ok(newsList);
        }

        [HttpPost("clear-cache")]
        public IActionResult ClearCache()
        {
            _cache.Remove("newsList");
            return Ok(new { message = "Cache cleared" });
        }
    }
}
