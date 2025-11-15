using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    public class AuthController : Controller
    {
        // Hiển thị form login
        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Trả về login.html (cần convert thành login.cshtml nếu dùng MVC View)
        }

        // Xử lý đăng nhập form POST
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Ở đây chỉ demo đơn giản: username = "HonggNhungg", password = "123456"
            if(username == "HonggNhungg" && password == "123456")
            {
                // Đăng nhập thành công, chuyển đến trang index
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Đăng nhập thất bại, trả về login với thông báo lỗi
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return View();
            }
        }

        // Logout đơn giản
        public IActionResult Logout()
        {
            // Xử lý logout nếu cần (xóa session,...)
            return RedirectToAction("Login");
        }
    }
}
