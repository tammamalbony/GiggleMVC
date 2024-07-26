using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Giggle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorController : ControllerBase
    {
        [Route("error")]
        public IActionResult HandleError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;

            if (exception != null)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine(exception?.Message);

                // Return a JSON response with error details
                var errorResponse = new
                {
                    statusCode = 500,
                    message = "An unexpected error occurred.",
                    detail = exception.Message
                };

                return StatusCode(500, errorResponse);
            }
            else
            {
                // Return a JSON response indicating success
                var successResponse = new
                {
                    statusCode = 200,
                    message = "No errors occurred."
                };

                return Ok(successResponse);
            }
        }

        [HttpGet("check")]
        public IActionResult Check()
        {
            // Example of a method that may trigger an error
            throw new Exception("This is a test exception.");
        }
    }
}
