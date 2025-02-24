using Microsoft.Extensions.Logging;
using Products.Infrastructure.Interfaces.Caching;
using StackExchange.Redis;
using System.Buffers;
using System.Text.Json;

namespace Products.Infrastructure.Caching
{
    internal class GenericCacheService<T> : ICacheService<T> where T : class
    {
        private readonly ConnectionMultiplexer _connection;
        private readonly IDatabase _cache;
        private readonly ILogger _loger;
        private readonly string _redisPrefix;

        public GenericCacheService(ILogger loger, RedisCacheConfiguration redisCacheConfiguration)
        {
            _connection = redisCacheConfiguration.GetConnection();
            _cache = _connection.GetDatabase();
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
            const int batchSize = 1000;
            var keysBatch = ArrayPool<RedisKey>.Shared.Rent(batchSize);

            foreach (var endpoint in _connection.GetEndPoints())
            {
                var server = _connection.GetServer(endpoint);
                var keys = server.KeysAsync(pattern: _redisPrefix + "*");
                var index = 0;

                await foreach (var key in server.KeysAsync(pattern: _redisPrefix + "*"))
                {
                    keysBatch[index++] = key;

                    if (index >= batchSize)
                    {
                        await _cache.KeyDeleteAsync(keysBatch);
                        index = 0;
                    }
                }

                if (index > 0)
                    await _cache.KeyDeleteAsync(keysBatch[..index]);
            }

            ArrayPool<RedisKey>.Shared.Return(keysBatch);
        }

        private RedisKey GetRedisKey(string key)
        {
            return new RedisKey(key).Append(_redisPrefix);
        }
    }
}
