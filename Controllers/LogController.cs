using Microsoft.AspNetCore.Mvc;
using log4net;
using System.Reflection;

namespace LearnAspNetCore.Controllers
{
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

[HttpGet("test")]
public IActionResult TestError()
{
try
{
int a = 10, b = 0;
int c = a / b;
return Ok(c);
}
catch (Exception ex)
{
_logger.Error("Lỗi trong UserController.TestError", ex);
return StatusCode(500, "Đã xảy ra lỗi khi xử lý yêu cầu.");
}
}
}
}