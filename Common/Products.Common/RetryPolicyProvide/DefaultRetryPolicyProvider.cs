using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Products.Common.Kafka;

namespace Products.Common.RetryPolicyProvide
{
    public class DefaultRetryPolicyProvider : IRetryPolicyProvider
    {
        private readonly ILogger<DefaultRetryPolicyProvider> _logger;
        private readonly RetryPolicyConfig _config;

        public DefaultRetryPolicyProvider(ILogger<DefaultRetryPolicyProvider> logger, IOptions<RetryPolicyConfig> config)
        {
            _logger = logger;
            _config = config.Value ?? new RetryPolicyConfig();
        }

        public IAsyncPolicy GetKafkaConsumerPolicy()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: _config.RetryCount,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(_config.BaseDelaySeconds, retryAttempt)),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(exception,
                            "[KafkaConsumerRetry] Attempt {RetryCount} failed. Retrying in {Delay}s...",
                            retryCount, timeSpan.TotalSeconds);
                    });
        }

        public IAsyncPolicy GetKafkaProducerPolicy()
        {
            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    _config.CircuitBreakerFailureThreshold,
                    TimeSpan.FromSeconds(_config.BaseDelaySeconds),
                    onBreak: (exception, breakDelay) =>
                    {
                        _logger.LogError("[KafkaProducerCircuitBreaker] Circuit broken! Retrying in {Delay} seconds.",
                            breakDelay.TotalSeconds);
                    },
                    onReset: () =>
                    {
                        _logger.LogInformation("[KafkaProducerCircuitBreaker] Circuit reset.");
                    });

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: _config.RetryCount,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(_config.BaseDelaySeconds, retryAttempt)),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(exception,
                            "[KafkaProducerRetry] Attempt {RetryCount} failed. Retrying in {Delay}s...",
                            retryCount, timeSpan.TotalSeconds);
                    });

            return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
        }
    }
}
