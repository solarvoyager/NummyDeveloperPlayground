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
var nummyServiceUrl = builder.Configuration["Nummy:ServiceUrl"]
    ?? throw new InvalidOperationException("Nummy:ServiceUrl is not configured in appsettings.json.");
var applicationId = builder.Configuration["Nummy:ApplicationId"]
    ?? throw new InvalidOperationException("Nummy:ApplicationId is not configured in appsettings.json.");

// Nummy.CodeLogger config
builder.Services.AddNummyCodeLogger(options => 
{
    options.NummyServiceUrl = nummyServiceUrl;
    options.ApplicationId = applicationId;
});

// Nummy.ExceptionHandler config
dynamic errorResponse = new ExpandoObject();
errorResponse.success = false;
errorResponse.message = "error caught & logged by nummy exception handler";

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
    options.MaskHeaders = ["Authorization"];
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

// Order is intentional: HttpLogger wraps ExceptionHandler so that error
// responses produced by the exception middleware are also captured and logged.
app.UseNummyHttpLogger();
app.UseNummyExceptionHandler();
app.MapNummyHealthChecker();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();