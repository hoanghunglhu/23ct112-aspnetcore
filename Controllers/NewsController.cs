using LearnApiNetCore.Entity;
using LearnApiNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        private const string NEWS_CACHE_KEY = "news_cache";

        public NewsController(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        [HttpGet]
        public IActionResult GetNews()
        {
            if (_cache.TryGetValue(NEWS_CACHE_KEY, out List<News> cachedNews))
            {
                return Ok(new
                {
                    source = "cache",
                    count = cachedNews.Count,
                    data = cachedNews
                });
            }
            var allNews = _context.News.ToList();
            var firstThree = allNews.Take(3).ToList();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60),
                SlidingExpiration = TimeSpan.FromSeconds(30)
            };

            _cache.Set(NEWS_CACHE_KEY, firstThree, cacheOptions);

            return Ok(new
            {
                source = "database",
                count = firstThree.Count,
                data = firstThree
            });
        }
        [HttpGet("all")]
        public IActionResult GetAllNews()
        {
            var all = _context.News.ToList();
            return Ok(new
            {
                source = "database",
                count = all.Count,
                data = all
            });
        }
        [HttpDelete("clear-cache")]
        public IActionResult ClearCache()
        {
            _cache.Remove(NEWS_CACHE_KEY);
            return Ok(new { message = "Đã xóa cache thành công." });
        }
    }
}
