using System.Net;
using Client;
using Client.Errors;
using Client.ExceptionHandlers;
using Client.Extensions;
using Serilog;
using Client.Logging;
using FluentResults;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, config) =>
{
    config.ReadFrom.Configuration(context.Configuration)
        .Enrich.WithProperty("application", builder.Environment.ApplicationName)
        .Enrich.FromLogContext()
        .Enrich.With<RemovePropertiesEnricher>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<FallbackExceptionHandler>();
var app = builder.Build();

app.UseMiddleware<RequestLogContextMiddleware>();
app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/generate-random-logs", async (int count, ILogger<Program> logger) =>
{
    if (count <= 0 || count > 10000)
        return Results.BadRequest("Count must be between 1 and 10000");

    Random random = new();

    for (int i = 0; i < count; i++)
    {
        var level = random.Next(1, 4);
        switch (level)
        {
            case 1:
                logger.LogInformation("User performed");
                break;
            case 2:
                logger.LogWarning("User had warning");
                break;
            case 3:
                logger.LogError("User caused error");
                break;
        }

        await Task.Delay(10);
    }

    return Results.Ok();
});

app.MapPost("/result-fail", async (ILogger<Program> logger) =>
{
    var err1 = new ApiError("message 1", "Error.OperationFailed");
    var err2 = new ApiError("message 2", "Error.OperationFailed2");
    
    Result operationResult = 
        Result.Fail(err1).WithError(err2);

    if (operationResult.IsFailed)
        return operationResult.Errors.ToMinimalApiResponse(logger);    
    
    return Results.Ok();   
});

app.MapPost("/throw-exception", async () =>
{
    throw new NotImplementedException("test");
});

app.Run();