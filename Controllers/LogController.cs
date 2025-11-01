using Microsoft.AspNetCore.Mvc;
using LearnApiNetCore.Helpers;   // để dùng LogHelper
using System;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestLogController : ControllerBase
    {
        [HttpGet("ok")]
        public IActionResult TestOk()
        {
            // Ghi log thông tin bình thường
            LogHelper.Info("Test OK endpoint được gọi thành công.");
            return Ok("Đã ghi log thông tin OK (xem file log).");
        }

        [HttpGet("error")]
        public IActionResult TestError()
        {
            try
            {
                // Cố tình ném lỗi để test ghi exception
                throw new Exception("Lỗi giả lập trong TestError()");
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TestLogController.TestError()");
                return StatusCode(500, "Đã xảy ra lỗi — xem file log để kiểm tra.");
            }
        }
    }
}
