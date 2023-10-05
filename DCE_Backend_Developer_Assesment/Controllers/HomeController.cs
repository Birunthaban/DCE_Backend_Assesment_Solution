using Microsoft.AspNetCore.Mvc;

namespace DCE_Backend_Developer_Assesment.Controllers
{
    [Route("home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult HelloWorld() {
            return Ok("Hello World");
        }
    }
}
