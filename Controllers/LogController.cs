using Microsoft.AspNetCore.Mvc;

namespace LearnAspNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private static readonly NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpGet("test")]
        public IActionResult TestLogging()
        {
            try
            {
                throw new Exception("Demo lỗi ghi log với NLog");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Đã xảy ra lỗi trong TestLogging()");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
