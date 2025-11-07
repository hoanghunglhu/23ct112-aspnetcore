using LearnApiNetCore.Entity;
using LearnApiNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Cần thêm namespace này cho các hàm Async của EF Core
using Microsoft.Extensions.Logging; 
using System.Threading.Tasks; // Cần thêm namespace này cho Task/async/await

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserController> _logger; // Khai báo ILogger

        // 1. Inject ILogger và AppDbContext
        public UserController(AppDbContext context, ILogger<UserController> logger)
        {
            _context = context; 
            _logger = logger;
        }

        // Đã chuyển sang Async/Await
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // Sử dụng ToListAsync() cho hiệu năng bất đồng bộ
                var users = await _context.Users.ToListAsync(); 
                return Ok(users);
            }
            catch (Exception ex)
            {
                await LogExceptionAsync(ex, "Lỗi khi lấy danh sách người dùng.");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        // Phương thức minh họa Try-Catch và Log Exception BẤT ĐỒNG BỘ
        [HttpPost]
        public async Task<IActionResult> Create(UserModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Lỗi validation: Không cần log Exception, chỉ trả về Bad Request
                    return BadRequest(ModelState);
                }

                var user = new User
                {
                    name = model.name,
                    email = model.email,
                    // ... các trường khác
                };

                // Kiểm tra log
                // int zero = 0;
                // int result = 100 / zero;

                _context.Users.Add(user);
                await _context.SaveChangesAsync(); // Sử dụng SaveChangesAsync()

                return CreatedAtAction(nameof(GetById), new { id = user.id }, user);
            }
            catch (DbUpdateException dbEx) // Bắt lỗi liên quan đến DB (ví dụ: unique constraint)
            {
                // Lưu log Exception bất đồng bộ
                await LogExceptionAsync(dbEx, $"Lỗi DB khi tạo người dùng mới: {model.email}");
                return StatusCode(500, new { Message = "Database error. User might already exist." });
            }
            catch (Exception ex) // Bắt các lỗi khác (lỗi logic, lỗi hệ thống)
            {
                // Lưu log Exception bất đồng bộ
                await LogExceptionAsync(ex, $"Lỗi không xác định khi tạo người dùng: {model.email}");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        // ... Các phương thức GetById, Update, Delete cũng nên được chuyển sang async/await ...

        // Phương thức Log Exception Bất Đồng Bộ (Sử dụng Task.Run cho Multi-thread)
        // **Đây là nơi áp dụng Queue/Multi-thread/Async/Await**
        private async Task LogExceptionAsync(Exception ex, string message)
        {
            // Task.Run để đảm bảo việc ghi log (I/O) không block luồng API chính
            await Task.Run(() =>
            {
                _logger.LogError(ex, message);
            });
        }
    }
}