using Serilog.Core;
using Serilog.Events;

namespace Client.Logging;

public sealed class RemoveHttpScopePropertiesEnricher : ILogEventEnricher
{
    private static readonly string[] PropertyNames =
    [
        "RequestId",
        "RequestPath",
        "ConnectionId"
    ];

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        foreach (var propertyName in PropertyNames)
        {
            logEvent.RemovePropertyIfPresent(propertyName);
        }
    }
}
