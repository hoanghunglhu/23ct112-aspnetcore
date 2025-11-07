using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LearnApiNetCore.Services;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("log/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logger;

        public LogController(ILogService logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult CreateLog()
        {
            try
            {
                throw new Exception("Đây là một lỗi");
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, "Cố tình tạo lỗi");
            }
            return Ok();
        }
    }
}