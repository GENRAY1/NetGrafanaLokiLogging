using System.Net;
using FluentResults;

namespace Client.Extensions;

public static class ErrorHttpMapperExtensions
{
    public static IResult ToHttpErrorResponse(
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