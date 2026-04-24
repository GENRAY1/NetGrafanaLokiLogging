using System.Diagnostics;
using Serilog.Context;

namespace Client.Logging;

public class RequestLoggingContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString();
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var traceId = Activity.Current?.TraceId.ToString();
    
        using (LogContext.PushProperty("HttpMethod", context.Request.Method))
        using (LogContext.PushProperty("HttpPath", context.Request.Path))
        using (LogContext.PushProperty("Ip", ip))
        using (LogContext.PushProperty("TraceId", traceId))
        using (LogContext.PushProperty("UserAgent", userAgent))
        {
            await next(context);
        }
    }
}