namespace Products.Common.RetryPolicyProvide
{
    public record RetryPolicyConfig
    {
        public int RetryCount { get; set; } = 3;

        public int CircuitBreakerFailureThreshold { get; set; } = 5;

        public int CircuitBreakerDurationInMinutes { get; set; } = 1;

        public double BaseDelaySeconds { get; set; } = 2;
    }
}
