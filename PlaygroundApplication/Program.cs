using System.Dynamic;
using System.Net;
using Nummy.CodeLogger.Extensions;
using Nummy.ExceptionHandler.Extensions;
using Nummy.HttpLogger.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

const string nummyServiceUrl = "http://localhost:8082/";

const string applicationId = "5dd3ec25-5fb9-4f92-bf70-bff42c9da148";

builder.Services.AddNummyCodeLogger(options => 
{
    options.NummyServiceUrl = nummyServiceUrl;
    options.ApplicationId = applicationId;
});

dynamic errorResponse = new ExpandoObject();
errorResponse.success = false;
errorResponse.message = "error catched & logged by nummy exception handler";

builder.Services.AddNummyExceptionHandler(options =>
{
    options.HandleException = true;
    options.ResponseStatusCode = HttpStatusCode.Conflict;
    options.Response = errorResponse;
    options.ApplicationId = applicationId;
    options.NummyServiceUrl = nummyServiceUrl;
});

builder.Services.AddNummyHttpLogger(options =>
{
    options.EnableRequestLogging = true;
    options.EnableResponseLogging = true;
    options.ExcludeContainingPaths = ["swagger"];
    options.ApplicationId = applicationId;
    options.NummyServiceUrl = nummyServiceUrl;
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