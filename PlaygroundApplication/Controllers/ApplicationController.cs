using Microsoft.AspNetCore.Mvc;
using Nummy.CodeLogger.Data.Services;

namespace PlaygroundApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class ApplicationController : ControllerBase
{
    private readonly INummyCodeLoggerService _loggerService;

    public ApplicationController(INummyCodeLoggerService loggerService)
    {
        _loggerService = loggerService;
    }

    [HttpGet("Test")]
    public async Task<IActionResult> Get()
    {
        await _loggerService.LogInfoAsync("infoo", "gffd");
        await _loggerService.LogInfoAsync(new ArgumentNullException("tgkhkkjk"));

        return Ok("senede");
    }
}