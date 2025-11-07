using Microsoft.AspNetCore.Mvc;
using NLog;

namespace LearnAspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpGet("error")]
        public IActionResult TestError()
        {
            try
            {
                int x = 0;
                int y = 5 / x;
                return Ok(y);
            }
            catch (Exception ex)
            {
                logger.Error(ex, " Lỗi chia cho 0");
                return StatusCode(500, "Đã xảy ra lỗi!");
            }
        }
    }
}
