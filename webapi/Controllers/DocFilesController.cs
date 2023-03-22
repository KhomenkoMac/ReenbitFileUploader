using api;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

public record UserDocDto(string UserEmail, IFormFile UserDocxFile);

[ApiController]
[Route("[controller]")]
public class DocFilesController : ControllerBase
{
    private readonly AzureDocxUploaderService _docxUploader;

    public DocFilesController(AzureDocxUploaderService docxUploader)
    {
        _docxUploader = docxUploader;
    }

    [HttpPost("docs")]
    public async Task<IActionResult> Post([FromForm] UserDocDto request)
    {
        try
        {
            await _docxUploader.UploadFileAsStream(request);
        }
        catch (Exception e)
        {
            return NotFound(new
            {
                ErrorMessage = e.Message,
                Trace = e.StackTrace
            });
        }
        return Ok(new { message = "Your docs has been uploaded!" });
    }

    [HttpGet("check")]
    public string Check()
    {
        return "Api is up!";
    }
}