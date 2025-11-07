using LearnApiNetCore.Entity;
using LearnApiNetCore.Models;
using LearnApiNetCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AppDbContext context, IJwtService jwtService, ILogger<AuthController> logger)
        {
            _context = context;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            _logger.LogInformation("Attempting to register new user: {Username}", model.Username);
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
                _logger.LogError(ex, "Error occurred while registering user: {Username}", model?.Username);
                return StatusCode(500, new { message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (model == null)
            {
                _logger.LogWarning("Login called with empty model");
                return BadRequest(new { message = "Invalid request body" });
            }

            _logger.LogInformation("Login attempt for username: {Username}", model.Username);

            try
            {
                // Tìm user theo username
                var user = _context.Users.FirstOrDefault(u => u.username == model.Username);

                if (user == null)
                {
                    _logger.LogWarning("Login failed - user not found: {Username}", model.Username);
                    return BadRequest(new { message = "Username hoặc password không đúng" });
                }

                // Kiểm tra password
                var passwordOk = false;
                try
                {
                    passwordOk = BCrypt.Net.BCrypt.Verify(model.Password, user.password);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "BCrypt verify failed for user {Username}", model.Username);
                    return StatusCode(500, new { message = "Lỗi trong quá trình xác thực mật khẩu" });
                }

                if (!passwordOk)
                {
                    _logger.LogWarning("Login failed - invalid password for user: {Username}", model.Username);
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

                _logger.LogInformation("Login successful for user {Username}", model.Username);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception during login for user {Username}", model?.Username);
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
                _logger.LogError(ex, "Error validating token");
                return StatusCode(500, new { message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}