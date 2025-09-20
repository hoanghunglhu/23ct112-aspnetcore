using Microsoft.AspNetCore.Mvc;

namespace LearnApiNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //api/hello
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public int Get()
        {
            return 100;
        }
    }
}