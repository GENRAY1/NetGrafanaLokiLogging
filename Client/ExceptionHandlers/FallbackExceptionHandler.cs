using System.Net;
using Client.Errors;
using FluentResults;
using Microsoft.AspNetCore.Diagnostics;

namespace Client.ExceptionHandlers;

public sealed class FallbackExceptionHandler(ILogger<FallbackExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        IError[] errors =
        [
            new ApiError(
                "An unexpected error occurred", 
                "ExceptionHandler.Unexpected")
        ];

        await httpContext.Response.WriteAsJsonAsync(errors, cancellationToken);

        return true;
    }
}