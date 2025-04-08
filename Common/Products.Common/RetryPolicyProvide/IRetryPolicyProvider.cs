using Polly;
using Polly.Retry;

namespace Products.Common.Kafka
{
    public interface IRetryPolicyProvider
    {
        IAsyncPolicy GetKafkaConsumerPolicy();

        IAsyncPolicy GetKafkaProducerPolicy();
    }
}
