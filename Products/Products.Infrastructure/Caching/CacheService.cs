using Microsoft.Extensions.Logging;
using Products.Infrastructure.Interfaces.Caching;
using StackExchange.Redis;
using System.Text.Json;

namespace Products.Infrastructure.Caching
{
    internal class GenericCacheService<T> : ICacheService<T> where T : class
    {
        private readonly IDatabase _cache;
        private readonly ILogger _loger;
        private readonly string _redisPrefix;

        public GenericCacheService(ILogger loger, RedisCacheConfiguration redisCacheConfiguration)
        {
            var connection = redisCacheConfiguration.GetConnection();
            _cache = connection.GetDatabase();
            _loger = loger;
            var cacheValueType = typeof(T);
            _redisPrefix = $"{cacheValueType.Namespace}.{cacheValueType.Name}";
        }

        public async Task<IEnumerable<T>?> GetCacheAsync(string key)
        {
            var cachedData = await _cache.StringGetAsync(GetRedisKey(key));

            if (string.IsNullOrWhiteSpace(cachedData))
                return default;

            return JsonSerializer.Deserialize<List<T>>(cachedData);
        }

        public async Task SetCacheAsync(string key, IEnumerable<T> value, TimeSpan? expiry = null)
        {
            var serializedData = JsonSerializer.Serialize(value);
            await _cache.StringSetAsync(GetRedisKey(key), serializedData, expiry);
        }

        public async Task InvalidateCacheAsync()
        {
            throw new NotImplementedException();
        }

        private RedisKey GetRedisKey(string key)
        {
            return new RedisKey(key).Append(_redisPrefix);
        }
    }
}
