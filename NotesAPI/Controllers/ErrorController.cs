using Microsoft.AspNetCore.Mvc;

namespace NotesAPI.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/Home/Error")]
        [ApiExplorerSettings(IgnoreApi = true)] // Optional: Exclude from Swagger documentation
        public IActionResult Error()
        {
            return StatusCode(500, new { message = "An error occurred while processing the request." });
        }
    }

}
