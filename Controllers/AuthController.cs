using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq; // Cần thêm namespace này để dùng LINQ (FirstOrDefault, Any)

// Class mô hình User đã được đổi tên thành AppUser
public class AppUser
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class AuthController : Controller
{
    private readonly IWebHostEnvironment _env;
    
    // Danh sách tĩnh lưu trữ tạm thời, sử dụng AppUser
    private static List<AppUser> _users = new List<AppUser>();
    
    public AuthController(IWebHostEnvironment env)
    {
        _env = env;
    }

    // Trả về trang login
    [HttpGet("/login.html")]
    public IActionResult Login()
    {
        var path = System.IO.Path.Combine(_env.WebRootPath, "login.html");
        return PhysicalFile(path, "text/html");
    }

    // Trả về trang register
    [HttpGet("/register.html")]
    public IActionResult Register()
    {
        var path = System.IO.Path.Combine(_env.WebRootPath, "register.html");
        return PhysicalFile(path, "text/html");
    }

    // API xử lý đăng nhập
    [HttpPost("/api/auth/login")]
    public IActionResult LoginApi([FromBody] LoginRequest request)
    {
        // TÌM user trong danh sách tạm thời
        var user = _users.FirstOrDefault(u => 
            u.Email.ToLower() == request.Email.ToLower() && u.Password == request.Password
        );
        
        if (user != null)
        {
            return Ok(new { 
                success = true, 
                message = "Đăng nhập thành công!",
                user = new {
                    email = user.Email,
                    name = user.Name
                }
            });
        }
        
        return BadRequest(new { success = false, message = "Email hoặc mật khẩu không đúng!" });
    }

    // API xử lý đăng ký
    [HttpPost("/api/auth/register")]
    public IActionResult RegisterApi([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Name))
        {
            return BadRequest(new { success = false, message = "Vui lòng điền đầy đủ thông tin!" });
        }
        
        // KIỂM TRA xem email đã tồn tại chưa
        if (_users.Any(u => u.Email.ToLower() == request.Email.ToLower()))
        {
            return BadRequest(new { success = false, message = "Email đã được đăng ký!" });
        }
        
        // TẠO user mới và LƯU vào danh sách tạm thời
        var newUser = new AppUser // Sử dụng AppUser
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password 
        };

        _users.Add(newUser);
        
        return Ok(new { 
            success = true, 
            message = "Đăng ký thành công!",
            user = new {
                email = newUser.Email,
                name = newUser.Name
            }
        });
    }

    // API đăng xuất
    [HttpPost("/api/auth/logout")]
    public IActionResult Logout()
    {
        return Ok(new { success = true, message = "Đăng xuất thành công!" });
    }
}

// Models (Giữ nguyên)
public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}