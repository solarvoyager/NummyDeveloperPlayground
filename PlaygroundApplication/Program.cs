using System.Dynamic;
using System.Net;
using Nummy.CodeLogger.Extensions;
using Nummy.ExceptionHandler.Extensions;
using Nummy.HealthChecker.Entites;
using Nummy.HealthChecker.Extensions;
using Nummy.HttpLogger.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// From Nummy
const string nummyServiceUrl = "http://localhost:8082/";
const string applicationId = "909e2c2f-48d3-47d2-9971-c96334c2f12c";

// Nummy.CodeLogger config
builder.Services.AddNummyCodeLogger(options => 
{
    options.NummyServiceUrl = nummyServiceUrl;
    options.ApplicationId = applicationId;
});

// Nummy.ExceptionHandler config
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

// Nummy.HttpLogger config
builder.Services.AddNummyHttpLogger(options =>
{
    options.EnableRequestLogging = true;
    options.EnableResponseLogging = true;
    options.ExcludeContainingPaths = ["swagger"];
    options.MaskHeaders = ["Content-Type"];
    options.ApplicationId = applicationId;
    options.NummyServiceUrl = nummyServiceUrl;
});

// Nummy.HealthChecker config
builder.Services.AddNummyHealthChecker(options =>
{
    options.Path = "nummy/health";

    options.CheckAsync = async (sp, ct) =>
    {
        // Example: use services from DI
        // var db = sp.GetRequiredService<MyDbContext>();
        // await db.Database.ExecuteSqlRawAsync("SELECT 1", ct);

        // For now: simple “OK”
        return new NummyHealthResult
        {
            IsHealthy = true,
            Message = "Service is healthy"
        };
    };
});

var app = builder.Build();

app.UseNummyHttpLogger();
app.UseNummyExceptionHandler();
app.MapNummyHealthChecker();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();