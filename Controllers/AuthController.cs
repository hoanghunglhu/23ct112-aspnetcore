using LearnApiNetCore.Entity;
using LearnApiNetCore.Models;
using LearnApiNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthController(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            try
            {
                // Kiểm tra xem username đã tồn tại chưa
                if (_context.Users.Any(u => u.username == model.Username))
                {
                    return BadRequest(new { message = "Username đã tồn tại" });
                }

                // Kiểm tra xem email đã tồn tại chưa
                if (_context.Users.Any(u => u.email == model.Email))
                {
                    return BadRequest(new { message = "Email đã tồn tại" });
                }

                // Mã hóa password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // Tạo user mới
                var user = new User
                {
                    username = model.Username,
                    password = hashedPassword,
                    name = model.Name,
                    email = model.Email,
                    phone = model.Phone,
                    address = model.Address,
                    gender = model.Gender,
                    birthday = model.Birthday
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                // Tạo token
                var token = _jwtService.GenerateToken(user);

                var response = new AuthResponse
                {
                    Token = token,
                    Expires = DateTime.UtcNow.AddDays(7),
                    User = new UserInfoResponse
                    {
                        Id = user.id,
                        Username = user.username,
                        Name = user.name,
                        Email = user.email
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                // Tìm user theo username
                var user = _context.Users.FirstOrDefault(u => u.username == model.Username);
                
                if (user == null)
                {
                    return BadRequest(new { message = "Username hoặc password không đúng" });
                }

                // Kiểm tra password
                if (!BCrypt.Net.BCrypt.Verify(model.Password, user.password))
                {
                    return BadRequest(new { message = "Username hoặc password không đúng" });
                }

                // Tạo token
                var token = _jwtService.GenerateToken(user);

                var response = new AuthResponse
                {
                    Token = token,
                    Expires = DateTime.UtcNow.AddDays(7),
                    User = new UserInfoResponse
                    {
                        Id = user.id,
                        Username = user.username,
                        Name = user.name,
                        Email = user.email
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost("validate-token")]
        public IActionResult ValidateToken([FromBody] string token)
        {
            try
            {
                var isValid = _jwtService.ValidateToken(token);
                
                if (isValid)
                {
                    var userId = _jwtService.GetUserIdFromToken(token);
                    var user = _context.Users.Find(userId);
                    
                    if (user != null)
                    {
                        return Ok(new 
                        { 
                            valid = true, 
                            user = new UserInfoResponse
                            {
                                Id = user.id,
                                Username = user.username,
                                Name = user.name,
                                Email = user.email
                            }
                        });
                    }
                }

                return Ok(new { valid = false });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}