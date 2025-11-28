using Microsoft.AspNetCore.Mvc;
using Nummy.CodeLogger.Data.Entitites;
using Nummy.CodeLogger.Data.Services;

namespace PlaygroundApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class ApplicationController(INummyCodeLoggerService loggerService) : ControllerBase
{
    [HttpGet("Test")]
    public async Task<IActionResult> Get()
    {
        await loggerService.LogInfoAsync("This is info title", "Description of me");
        await loggerService.LogAsync(NummyCodeLogLevel.Fatal, new ArgumentNullException("fatal somethin happened"));
        await loggerService.LogErrorAsync(new ArgumentNullException("test error"));

        return Unauthorized("No acess");
    }
    
    [HttpDelete("Test2")]
    public async Task<IActionResult> Get2()
    {
        throw new ArgumentOutOfRangeException("rreses");
    }
}