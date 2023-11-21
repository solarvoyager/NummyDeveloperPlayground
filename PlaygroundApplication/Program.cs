using Nummy.CodeLogger.Extensions;
using Nummy.CodeLogger.Models;
using Nummy.HttpLogger.Extensions;
using Nummy.HttpLogger.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

const string db =
    "Host=localhost;Port=5432;Database=nummy_db;Username=postgres;Password=postgres;IncludeErrorDetail=true;";

builder.Services.AddNummyHttpLogger(options =>
{
    // Configure options here
    // Example: 
    options.EnableRequestLogging = true;
    options.EnableResponseLogging = true;
    options.ExcludeContainingPaths = new[] { "swagger" };
    options.DatabaseType = NummyHttpLoggerDatabaseType.PostgreSql;
    options.DatabaseConnectionString = db;
});

builder.Services.AddNummyCodeLogger(options =>
{
    // Configure options here
    // Example: 
    options.DatabaseType = NummyCodeLoggerDatabaseType.PostgreSql;
    options.DatabaseConnectionString = db;
});

var app = builder.Build();

app.UseNummyHttpLogger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();