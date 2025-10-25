using LearnApiNetCore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        public NewsController(AppDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }
        [HttpGet]
        public IActionResult GetAllNews()
        {
            if (!_cache.TryGetValue("newsList", out List<News> newsList))
            {
                newsList = _context.News.ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60));

                _cache.Set("newsList", newsList, cacheOptions);
            }

            return Ok(newsList);
        }
        [HttpDelete]
        public IActionResult DeleteCache() 
        {
            _cache.Remove("newsList");
            return Ok("Cache cleared");
        }
    }
}