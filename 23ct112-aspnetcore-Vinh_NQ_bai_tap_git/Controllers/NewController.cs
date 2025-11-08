using LearnApiNetCore.Entity;
using LearnApiNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string? cacheKey = "newList";

        public NewController(IMemoryCache cache, AppDbContext context)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        public IActionResult GetNews()
        {
            if (!_cache.TryGetValue(cacheKey, out List<User> newList))
            {
                newList = _context.Users.ToList();

                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                _cache.Set(cacheKey, newList, cacheOptions);
            }

            return Ok(newList);

        }
        [HttpPost("clear-cache")]
        public IActionResult ClearCache()
        {
            _cache.Remove(cacheKey);
            return Ok(new { message = "Cache đã được xóa thành công!" });
        }
    }
}