using Microsoft.AspNetCore.Mvc;
using NotNervous.Server.Models;

namespace NotNervous.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrepareController : Controller
    {

        [HttpPost("/resume")]
        public async Task<IActionResult> ReceiveResume()
        {
            HttpContext.Request.EnableBuffering();

            byte[] buffer = new byte[4096];
            while (HttpContext.Request.Body.Read(buffer) > 0)
            {
                await Task.Delay(100);
            }

            //var interview = InterviewModel.Create

            //using var reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8, leaveOpen: true);

            //byte[] audioBase64 = Convert.FromBase64String((string)body.resume);
            // Process the byte array as needed
            return Ok();
        }
    }
}
