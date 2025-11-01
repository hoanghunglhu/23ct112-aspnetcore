using LearnApiNetCore.Entity;
using LearnApiNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using log4net;
namespace LearnApiNetCore.Controllers
{
  [ApiController]
  [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TestController));

        [HttpGet("demo")]
        public IActionResult GetDemo()
        {
            try
            {
                log.Info("Nhận yêu cầu GET /test/demo");
                // throw new Exception("Lỗi thử nghiệm");
                return Ok(new { message = "Test thành công!" });
            }
            catch (Exception ex)
            {
                log.Error("Đã xảy ra lỗi:", ex);
                return StatusCode(500, "Đã ghi log lỗi vào file!");
            }
        }
    }
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