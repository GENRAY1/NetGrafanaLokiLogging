using System.Diagnostics;
using Client.Extensions;
using Serilog.Context;

namespace Client.Logging;

public class RequestLogContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        using (LogContext.PushProperty("ClientId", context.Request.GetClientId()))
        using (LogContext.PushProperty("TraceId", Activity.Current?.TraceId.ToString()))
        using (LogContext.PushProperty("RequestMethod", context.Request.Method))
        {
            await next(context);
        }
    }
}