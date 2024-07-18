using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Giggle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorController : ControllerBase
    {
        [HttpGet("Check")]
        public IActionResult Check()
        {
            // Simulate an error for testing
            throw new Exception("Test error occurred!");

            // In a real application, this might return some status or check the state
            // return Ok(new { status = "No errors" });
        }
    }
}
