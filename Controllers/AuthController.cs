using Microsoft.AspNetCore.Mvc;

namespace LearnApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Tài khoản mẫu
        private const string demoUser = "admin";
        private const string demoPass = "123";

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (model.Username == demoUser && model.Password == demoPass)
            {
                return Ok(new
                {
                    success = true,
                    message = "Đăng nhập thành công!"
                });
            }

            return Unauthorized(new
            {
                success = false,
                message = "Sai tài khoản hoặc mật khẩu!"
            });
        }
    }

    // Model nhận dữ liệu login
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
