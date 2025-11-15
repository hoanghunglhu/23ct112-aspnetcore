using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations; // Cần thiết cho [Required]

namespace MyWebApp.Controllers
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string? Password { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")] 
    public class AuthController : ControllerBase
    {
        public AuthController()
        {
            
        }

        [HttpPost("login")] 
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (model.Username == "admin" && model.Password == "123")
            {
                return Ok(new { message = "Đăng nhập thành công!" });
            }
            else
            {
                return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu không đúng." });
            }
        }
    }
}