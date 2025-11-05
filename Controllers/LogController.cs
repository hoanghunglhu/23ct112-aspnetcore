using Microsoft.AspNetCore.Mvc;
using LearnApiNetCore.Helpers;
using System;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestLogController : ControllerBase
    {
        [HttpGet("error")]
        public IActionResult TestError()
        {
            try
            {
                // Cố tình tạo lỗi
                int x = 0;
                int y = 10 / x;
                return Ok("Không bao giờ chạy đến đây");
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TestLogController.TestError()");
                return StatusCode(500, "✅ Lỗi đã được ghi vào file log.");
            }
        }
    }
    
}
