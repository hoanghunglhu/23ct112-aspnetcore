using System;
using LearnApiNetCore.Entity;
using LearnApiNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Log4NetManager = log4net.LogManager;
using NLogManager = NLog.LogManager;
using log4net;
using System;

namespace LearnApiNetCore.Controllers
{
  // -------------------------- TEST CONTROLLER --------------------------
  [ApiController]
    [Route("api/[controller]")]
    public class TestLog4NetController : ControllerBase
    {
        private static readonly ILog log4 = LogManager.GetLogger(typeof(TestLog4NetController));

        [HttpGet("test")]
        public IActionResult TestLog4Net()
        {
            try
            {
                log4.Info("Đang xử lý request bằng log4net");
                int a = 10, b = 0;
                int c = a / b; // lỗi giả lập
                return Ok("Thành công");
            }
            catch (Exception ex)
            {
                log4.Error("Lỗi khi xử lý request bằng log4net", ex);
                return StatusCode(500, "Lỗi nội bộ server (log4net).");
            }
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class TestNLogController : ControllerBase
    {
        private static readonly NLog.ILogger nlogger = NLog.LogManager.GetCurrentClassLogger();

        [HttpGet("test")]
        public IActionResult TestNLog()
        {
            try
            {
                nlogger.Info("Đang xử lý request bằng NLog");
                int a = 10, b = 0;
                int c = a / b;
                return Ok("Thành công");
            }
            catch (Exception ex)
            {
                nlogger.Error(ex, "Lỗi khi xử lý request bằng NLog");
                return StatusCode(500, "Lỗi nội bộ server (NLog).");
            }
        }
    }
    // -------------------------- USER CONTROLLER --------------------------
  [ApiController]
  [Route("api/[controller]")]
  //api/hello
  public class UserController : ControllerBase
  {
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
      _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
      var users = _context.Users.ToList();
      return Ok(users);
    }
    
    [HttpPost]
    public IActionResult Create(UserModel model)
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
      _context.SaveChanges();

      return CreatedAtAction(nameof(GetById), new { id = user.id }, user);
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