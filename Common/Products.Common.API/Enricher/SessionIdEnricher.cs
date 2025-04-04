using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace Products.Common.API.Enricher
{
    internal class SessionIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var traceId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString();
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(Const.SessionIdKey, traceId));
        }
    }
}
