using cultured.server.Helpers;
using Microsoft.AspNetCore.Mvc;
using cultured.server.Infrastructure.Models.Main;
using cultured.server.Infrasructure.Models.Helpers;
using cultured.server.Infrastructure.Managers.Main;
using cultured.server.Infrastructure.Models.Culture;
using cultured.server.Infrastructure.Managers.Culture;

namespace cultured.server.Controllers
{
    [ApiController]
    [Route("cms")]
    public class CMSController : ControllerBase
    {
        [HttpPost]
        [Route("manage/category")]
        public async Task<IActionResult> ManageCategory([FromBody] Category model)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext))
                {
                    var result = await new CultureManager().ManageCategory(model);
                    return Ok(result);
                }
                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("manage/character")]
        public async Task<IActionResult> ManageCharacter([FromBody] Character model)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext))
                {
                    var result = await new CultureManager().ManageCharacter(model);
                    return Ok(result);
                }
                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("manage/background")]
        public async Task<IActionResult> ManageBackground([FromBody] Backgrounds model)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext))
                {
                    var result = await new MainManager().ManageBackground(model);
                    return Ok(result);
                }
                return StatusCode(500, "Authorization failed");
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
                if (AuthHelpers.Authorize(HttpContext))
                {
                    var result = await new CultureManager().FilterCharacters(filter);
                    return Ok(result);
                }
                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("filter/backgrounds")]
        public async Task<IActionResult> FilterBackgrounds([FromBody] Filter filter)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext))
                {
                    var result = await new MainManager().FilterBackgrounds(filter);
                    return Ok(result);
                }
                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("delete/character")]
        public async Task<IActionResult> DeleteCharacter([FromBody] int ID)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext))
                {
                    var result = await new CultureManager().DeleteCharacter(ID);
                    return Ok(result);
                }
                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("delete/background")]
        public async Task<IActionResult> DeleteBackgrounds([FromBody] int ID)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext))
                {
                    var result = await new MainManager().DeleteBackground(ID);
                    return Ok(result);
                }
                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
