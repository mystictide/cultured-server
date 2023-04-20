using cultured.server.Helpers;
using Microsoft.AspNetCore.Mvc;
using cultured.server.Infrastructure.Managers.Main;
using cultured.server.Infrasructure.Models.Helpers;
using cultured.server.Infrastructure.Managers.Culture;

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

        [HttpGet]
        [Route("get/character")]
        public async Task<IActionResult> GetCharacter([FromQuery] int? ID, [FromQuery] string? Name)
        {
            try
            {
                var result = await new MainManager().GetCharacter(ID, Name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("get/categories")]
        public async Task<IActionResult> GetCategory([FromQuery] bool? main, [FromQuery] bool? prev, [FromQuery] int? parentid)
        {
            try
            {
                var result = await new MainManager().GetCategory(main: main, prev: prev, parentid: parentid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("filter/characters")]
        public async Task<IActionResult> FilterCharacters([FromBody] Filter filter)
        {
            try
            {
                var result = await new CultureManager().FilterCharacters(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
