using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Products.Common.Kafka.EventArg;

namespace Products.Common.Kafka.HealthChecks
{
    internal partial class KafkaHealthCheck : IHealthCheck
    {
        private readonly IKafkaProducer _producer;
        private readonly ILogger<KafkaHealthCheck> _logger;

        public KafkaHealthCheck(
            IKafkaProducer producer,
            ILogger<KafkaHealthCheck> logger)
        {
            _producer = producer;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _producer.ProduceAsync<PingEvent>(new PingEvent(), cancellationToken);

                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                if (ex is not OperationCanceledException)
                    LogKafkaError(_logger, ex);

                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
        }

        [LoggerMessage(
            EventId = 1,
            Level = LogLevel.Critical,
            Message = "Kafka healthcheck failed.")]
        private static partial void LogKafkaError(ILogger logger, Exception exception);
    }
}
