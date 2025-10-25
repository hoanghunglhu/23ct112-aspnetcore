using LearnAspNetCore.DbContext;   // <-- Using DbContext
using LearnAspNetCore.Models;      // <-- Using Model
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq; // <-- Cần cho .ToList()

namespace LearnAspNetCore.Controllers
{
    [Route("news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly NewsDbContext _context; // Inject DbContext
        private const string CACHE_KEY = "NewsList";

        // Constructor: Inject cả IMemoryCache và NewsDbContext
        public NewsController(IMemoryCache cache, NewsDbContext context)
        {
            _cache = cache;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetNews()
        {
            // Thêm '?' để sửa lỗi warning nullable (CS8600)
            if (_cache.TryGetValue(CACHE_KEY, out List<New>? cachedNewsList))
            {
                // Cache Hit: Trả về dữ liệu từ cache
                Console.WriteLine("Cache Hit: Tra ve du lieu tu cache.");
                return Ok(cachedNewsList);
            }

            // Cache Miss: Lấy dữ liệu từ database
            Console.WriteLine("Cache Miss: Lay du lieu moi tu Database.");

            // Sửa lỗi CS0103: Thay thế code giả bằng truy vấn CSDL
            List<New> newsList = _context.News.ToList();

            // 3. Cache kết quả trong 60 giây
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));

            // Lưu dữ liệu mới vào cache
            _cache.Set(CACHE_KEY, newsList, cacheOptions);

            return Ok(newsList);
        }

        [HttpPost("clear-cache")]
        public IActionResult ClearCache()
        {
            _cache.Remove(CACHE_KEY);
            Console.WriteLine("Da xoa cache.");
            
            // Đã xóa dòng _isFirstCall = true; (Sửa lỗi CS0414)

            return Ok("Đã xóa cache thành công.");
        }
    }
}