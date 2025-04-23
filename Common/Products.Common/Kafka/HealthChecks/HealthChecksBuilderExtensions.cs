using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Products.Common.Kafka.HealthChecks
{
    public static class HealthChecksBuilderExtensions
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);
        private const string DefaultName = "kafka";

        public static IHealthChecksBuilder AddKafka(
            this IHealthChecksBuilder builder,
            string? name = null,
            TimeSpan? timeout = null)
        {
            builder.Services.AddSingleton<KafkaHealthCheck>();

            return builder.Add(new HealthCheckRegistration(
                name ?? DefaultName,
                provider => provider.GetRequiredService<KafkaHealthCheck>(),
                default,
                default,
                timeout ?? DefaultTimeout));
        }
    }
}
