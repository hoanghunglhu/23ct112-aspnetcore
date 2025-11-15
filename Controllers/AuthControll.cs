using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Giả lập danh sách người dùng
        private static readonly List<User> Users = new List<User>
        {
            new User { Username = "admin", Password = "admin123" },
            new User { Username = "user", Password = "user123" }
        };

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Kiểm tra user
            var user = Users.FirstOrDefault(u => 
                u.Username == request.Username && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Tên tài khoản hoặc mật khẩu không đúng" });
            }

            // Trả về token giả lập (thực tế nên tạo JWT)
            return Ok(new { message = "Đăng nhập thành công", token = "fake-jwt-token" });
        }

        // POST: api/auth/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Ở đây chỉ là ví dụ, logout thực sự sẽ xóa session hoặc token
            return Ok(new { message = "Đăng xuất thành công" });
        }
    }

    // Model Login Request
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // Model User
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
