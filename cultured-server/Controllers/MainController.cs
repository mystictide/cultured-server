using Microsoft.AspNetCore.Mvc;
using cultured.server.Infrastructure.Managers.Main;

namespace cultured.server.Controllers
{
    [ApiController]
    [Route("main")]
    public class MainController : ControllerBase
    {
        [HttpGet]
        [Route("get/background")]
        public async Task<IActionResult> GetBackgroundImage()
        {
            try
            {
                var result = await new MainManager().GetBackgroundImage();
                return Redirect(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
