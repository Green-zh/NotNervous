using Microsoft.AspNetCore.Mvc;
using NotNervous.Server.Providers;

namespace NotNervous.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DialogController : Controller
    {
        private DialogProvider dialogProvider;

        public DialogController(DialogProvider dialogProvider)
        {
            this.dialogProvider = dialogProvider;
        }

        [Route("/ws")]
        public async Task Get()
        {   
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                await dialogProvider.Process(HttpContext);
            }   
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        [HttpGet("hello")]
        public IActionResult Hello()
        {
            return Ok("Hello from DialogController");
        }
    }
}
    