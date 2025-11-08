using Microsoft.AspNetCore.Mvc;
using log4net;

namespace LearnApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(HomeController));

        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            try
            {
                throw new Exception("Giả lập lỗi để test gửi email!");
            }
            catch (Exception ex)
            {
                logger.Error("Đã xảy ra lỗi nghiêm trọng", ex);
                return StatusCode(500, "Lỗi đã được ghi log và gửi email.");
            }
        }
    }
}
