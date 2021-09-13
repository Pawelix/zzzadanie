using API.DTO;
using API.Enums;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class TextController : ControllerBase
    {
        public ITextService _service { get; set; }
        public TextController(ITextService service)
        {
            _service = service;
        }
        

        [HttpGet]
        [Route("Prepare")]
        public ActionResult<TextInfoDTO> Prepare([FromQuery] string input)
        {
            var result = default(TextInfoDTO);

            try
            {
                result = _service.PrepareText(input);
                Log.Information($"Operation went {LogMessages.Successfull}, Submited data is: {input}, Results: Unique characters: {result.UniqueCharCount}, Total characters: {result.CharCount}, Total words: {result.WordCount}");
                return Ok(result);
            }
            catch (System.Exception e)
            {
                Log.Error($"An {LogMessages.Error} occured while processing your request. Error info: {e.Message}");
                return BadRequest(e.Message);
            }
        }
    }
}