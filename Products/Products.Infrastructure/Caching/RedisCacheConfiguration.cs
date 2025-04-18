using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Products.Infrastructure.Caching
{
    public class RedisCacheConfiguration
    {
        private readonly ILogger<RedisCacheConfiguration> _loger;
        private readonly IConfiguration _configuration;

        public RedisCacheConfiguration(ILogger<RedisCacheConfiguration> loger, IConfiguration configuration)
        {
            _loger = loger;
            _configuration = configuration;
        }

        public ConnectionMultiplexer GetConnection()
        {
            var config = GetConfigurationOptions();
            return ConnectionMultiplexer.Connect(config);
        }

        private ConfigurationOptions GetConfigurationOptions()
        {
            var redisConnectionString = _configuration.GetSection("Redis:ConnectionString").Value;

            if (string.IsNullOrWhiteSpace(redisConnectionString))
            {
                _loger.LogError("Redis connection string is missing from configuration.");
            }

            return ConfigurationOptions.Parse(redisConnectionString ?? string.Empty);
        }
    }
}
