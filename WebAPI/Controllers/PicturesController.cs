using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebAPI.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly IPictureHandler _pictureHandlerService;

        public PicturesController(IPictureHandler pictureHandlerService)
        {
            _pictureHandlerService = pictureHandlerService;
        }

        [HttpPost("upload")]
        public async Task<ActionResult> UploadPicture(IFormFile picture)
        {
            try
            {
                bool res = await _pictureHandlerService.Save(picture);
                if (res)
                    return Ok(new { Message = "Successfull upload of the picture!" });
                else
                    return BadRequest(new { Error = "Picture with that name already exists!!" });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = e.Message });
            }
        }

        [HttpGet("get-all")]
        public async Task<ActionResult> GetAllPictures()
        {
            try
            {
                List<string> res = await _pictureHandlerService.GetAll();
                return Ok(new { Output = res });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = e.Message });
            }
        }

        [HttpDelete("{name}")]
        public async Task<ActionResult> DeleteByName(string name)
        {
            try
            {
                bool res = await _pictureHandlerService.DeleteByName(name);
                if (res)
                    return Ok(new { Message = "Successfully deleted the picture!" });
                else
                    return BadRequest(new { Error = $"There is no picture with the name:{name}!" });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = e.Message });
            }
        }

        [HttpGet("{name}")]
        public async Task<ActionResult> GetByName(string name)
        {
            try
            {
                var res = await _pictureHandlerService.GetByName(name);
                if (res != null)
                    return Ok(res);
                else
                    return BadRequest(new { Error = $"Picture with name:{name} does not exist!" });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = e.Message });
            }
        }
    }
}
