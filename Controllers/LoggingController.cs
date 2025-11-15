using Microsoft.AspNetCore.Mvc;
using log4net;
using System.Reflection;

namespace LearnApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggingController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        // nằm trong 23ct112-aspnetcore\bin\Debug\net8.0\Logs
        [HttpGet("test")]
        public IActionResult TestLogging()
        {
            try
            {
                log.Info("Hệ thống hoạt động bình thường");
                log.Warn("Cảnh báo: Có thể có lỗi nhỏ");
                throw new Exception("Giả lập lỗi test log4net");
            }
            catch (Exception ex)
            {
                log.Error(" Lỗi đã xảy ra", ex);
                return StatusCode(500, "Đã ghi log lỗi vào file log4net");
            }
        }
    }
}
