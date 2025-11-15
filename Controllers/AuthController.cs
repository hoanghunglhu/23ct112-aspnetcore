using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // Mô phỏng user demo
        private readonly List<User> users = new()
        {
            new User{ Username = "admin", Password = "12345" },
            new User{ Username = "user", Password = "54321" }
        };

        // POST api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Username và password không được để trống" });
            }

            var user = users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);

            if (user == null)
                return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu sai" });

            // Nếu dùng JWT, bạn có thể tạo token ở đây
            // Hiện tại demo trả về user info
            return Ok(new { message = "Đăng nhập thành công", username = user.Username });
        }
    }

    // Class nhận dữ liệu từ body
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // Class user demo
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
