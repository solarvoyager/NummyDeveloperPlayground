using Microsoft.AspNetCore.Mvc;
using Nummy.CodeLogger.Data.Entitites;
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
        await _loggerService.LogInfoAsync("This is info title", "Description of me");
        await _loggerService.LogAsync(NummyCodeLogLevel.Fatal, new ArgumentNullException("asda"));
        //await _loggerService.LogErrorAsync(new ArgumentNullException("asda"));

        throw new ArgumentOutOfRangeException("rreses");
        //return Ok("Test response");
    }
}