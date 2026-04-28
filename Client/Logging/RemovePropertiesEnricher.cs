using Serilog.Core;
using Serilog.Events;

namespace Client.Logging;

public sealed class RemovePropertiesEnricher : ILogEventEnricher
{
    private static readonly string[] PropertyNames =
    [
        "RequestId",
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
