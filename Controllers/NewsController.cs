using LearnApiNetCore.Entity;
using LearnApiNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            List<News> threeNews;

            if (!_cache.TryGetValue(NEWS_CACHE_KEY, out threeNews))
            {
                threeNews = _context.News.Take(3).ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));

                _cache.Set(NEWS_CACHE_KEY, threeNews, cacheOptions);
            }

            return Ok(new
            {
                cacheKey = NEWS_CACHE_KEY,
                data = threeNews
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
