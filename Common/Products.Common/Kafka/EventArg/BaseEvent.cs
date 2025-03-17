using System.Diagnostics;

namespace Products.Common.Kafka.EventArg
{
    public abstract class BaseEvent
    {
        public string Version { get; init; } = "1.0"; 
        public Guid EventId { get; init; } = Guid.NewGuid(); 
        public DateTime Timestamp { get; init; } = DateTime.UtcNow; 
        public string EventType { get; init; } 
        public string TraceId { get; init; } = Activity.Current?.Id ?? Guid.NewGuid().ToString();

        protected BaseEvent()
        {
            EventType = GetType().Name;
        }
    }
}
