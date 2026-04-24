using Serilog;
using Client.Logging;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("application", builder.Environment.ApplicationName)
    .Enrich.FromLogContext()
    .Enrich.With<RemoveHttpScopePropertiesEnricher>()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseMiddleware<RequestLoggingContextMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/generate-random-logs", async (int count, ILogger<Program> logger) =>
{
    if (count <= 0 || count > 10000)
        return Results.BadRequest("Count must be between 1 and 10000");

    Random random = new();
    Guid userId = Guid.NewGuid();


    for (int i = 0; i < count; i++)
    {
        DateTime dateTime = DateTime.UtcNow;

        var level = random.Next(1, 4);
        switch (level)
        {
            case 1:
                logger.LogInformation("User {UserId} performed", userId);
                break;
            case 2:
                logger.LogWarning("User {UserId} had warning", userId);
                break;
            case 3:
                logger.LogError(new NotImplementedException(), "User {UserId} caused error", userId);
                break;
        }

        await Task.Delay(10);
    }

    return Results.Ok();
});

app.Run();