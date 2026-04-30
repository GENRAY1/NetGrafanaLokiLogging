using System.Net;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Client.Extensions;

public static class ErrorHttpMapperExtensions
{
    /// <summary>
    /// FOR API CONTROLLER
    /// </summary>
    public static ActionResult ToApiResponse(
        this IReadOnlyList<IError> errors,
        ILogger logger,
        HttpStatusCode status = HttpStatusCode.BadRequest)
    {
        string msgs = string.Join("; ", errors.Select(e => e.Message));

        logger.LogWarning("Operation failed. Messages: {Messages}", msgs);

        return new ObjectResult(errors)
        {
            StatusCode = (int)status
        };
    }
    
    /// <summary>
    /// FOR MINIMAL API
    /// </summary>
    public static IResult ToMinimalApiResponse(
        this IReadOnlyList<IError> errors,
        ILogger logger,
        HttpStatusCode failureStatus = HttpStatusCode.BadRequest)
    {
        string msgs = string.Join("; ", errors.Select(e => e.Message));

        logger.LogWarning(
            "Operation failed. Messages: {Messages}", msgs);

        return Results.Json(errors, statusCode: (int)failureStatus);
    }
}