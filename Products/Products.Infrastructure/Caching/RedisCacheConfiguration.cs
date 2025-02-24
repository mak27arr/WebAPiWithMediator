using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Products.Infrastructure.Caching
{
    public class RedisCacheConfiguration
    {
        private readonly IConfiguration _configuration;

        public RedisCacheConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ConnectionMultiplexer GetConnection()
        {
            return ConnectionMultiplexer.Connect(GetConfigurationOptions());
        }

        private ConfigurationOptions GetConfigurationOptions()
        {
            var redisConnectionString = _configuration.GetSection("Redis:ConnectionString").Value;
            return ConfigurationOptions.Parse(redisConnectionString);
        }
    }
}
