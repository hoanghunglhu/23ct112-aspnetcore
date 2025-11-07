using LearnApiNetCore.Entity;
using LearnApiNetCore.Models;
using LearnApiNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;

namespace LearnApiNetCore.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  //api/hello
  public class UserController : ControllerBase
  {
    private readonly AppDbContext _context;
     private readonly EmailService _emailService;

    public UserController(AppDbContext context, EmailService emailService)
    {
      _context = context;
      _emailService = emailService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
      var users = _context.Users.ToList();
      return Ok(users);
    }

    [HttpPost]
public async Task<IActionResult> Create(UserModel model)
{
    try
    {
        var user = new User
        {
            name = model.name,
            email = model.email,
            phone = model.phone,
            address = model.address,
            birthday = model.birthday,
            gender = model.gender
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        //  Gửi email chào mừng
        await _emailService.SendEmailAsync(
            user.email,
            "Chào mừng bạn đến với hệ thống!",
            $"Xin chào {user.name},\n\nBạn đã được đăng ký thành công vào hệ thống!"
        );

        return CreatedAtAction(nameof(GetById), new { id = user.id }, user);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Lỗi khi tạo user");
        return StatusCode(500, "Có lỗi xảy ra khi tạo user!");
    }
}

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
      var user = _context.Users.Find(id);
      if (user == null)
      {
        return NotFound();
      }
      return Ok(user);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UserModel model)
    {
      var user = _context.Users.Find(id);
      if (user == null)
      {
        return NotFound();
      }

      user.name = model.name;
      user.email = model.email;
      user.phone = model.phone;
      user.address = model.address;
      user.birthday = model.birthday;
      user.gender = model.gender;
      _context.SaveChanges();
      return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      var user = _context.Users.Find(id);
      if (user == null)
      {
        return NotFound();
      }

      _context.Users.Remove(user);
      _context.SaveChanges();
      return NoContent();
    }
  }
}