using Microsoft.AspNetCore.Mvc;

namespace PlaygroundApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class ApplicationController : ControllerBase
{
    /*private readonly INummyCodeLoggerService _loggerService;

    public ApplicationController(INummyCodeLoggerService loggerService)
    {
        _loggerService = loggerService;
    }*/

    [HttpGet("Test")]
    public async Task<IActionResult> Get()
    {
        //await _loggerService.LogInfoAsync("infoo", "gffd");
        //await _loggerService.LogInfoAsync(new ArgumentNullException("tgkhkkjk"));

        throw new AggregateException();
        return Ok("Test response");
    }
}