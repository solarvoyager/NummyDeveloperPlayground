using System.Dynamic;
using System.Net;
using Nummy.CodeLogger.Extensions;
using Nummy.ExceptionHandler.Extensions;
using Nummy.HttpLogger.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

const string nummyServiceUrl =
    "http://localhost:8082/";

const string applicationId = "playground-application";


builder.Services.AddNummyCodeLogger(options => options.DsnUrl = dsnUrl);

dynamic errorResponse = new ExpandoObject();

errorResponse.success = false;
errorResponse.message = "error catched & logged by nummy exception handler";

builder.Services.AddNummyExceptionHandler(options =>
{
    options.HandleException = true; // if false, the app throws exceptions as a normal
    options.ResponseStatusCode = HttpStatusCode.Conflict;
    options.Response = errorResponse;
    options.DsnUrl = dsnUrl;
});

builder.Services.AddNummyHttpLogger(options =>
{
    // Configure options here
    // Example:
    options.EnableRequestLogging = true;
    options.EnableResponseLogging = true;
    options.ExcludeContainingPaths = ["swagger"];
    options.DsnUrl = dsnUrl;
});

var app = builder.Build();

app.UseNummyHttpLogger();
app.UseNummyExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();