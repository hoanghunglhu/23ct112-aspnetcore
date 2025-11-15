using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace YourProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Bộ nhớ tạm để lưu user đã đăng ký
        private static readonly List<UserData> RegisteredUsers = new List<UserData>();

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserData newUser)
        {
            if (string.IsNullOrWhiteSpace(newUser.Username) || string.IsNullOrWhiteSpace(newUser.Password))
            {
                return BadRequest(new { message = "Username và Password không được để trống" });
            }

            // Kiểm tra trùng tên
            if (RegisteredUsers.Any(u => u.Username == newUser.Username))
            {
                return BadRequest(new { message = "Username đã tồn tại" });
            }

            RegisteredUsers.Add(newUser);
            return Ok(new { message = "Đăng ký thành công!" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Tài khoản cố định
            if (request.Username == "Loc" && request.Password == "123")
            {
                return Ok(new { message = "Đăng nhập thành công!" });
            }

            // Kiểm tra user đã đăng ký
            if (RegisteredUsers.Any(u => u.Username == request.Username && u.Password == request.Password))
            {
                return Ok(new { message = "Đăng nhập thành công!" });
            }

            return Unauthorized(new { message = "Sai tên đăng nhập hoặc mật khẩu!" });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserData
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
