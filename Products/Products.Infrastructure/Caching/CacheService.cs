using Microsoft.Extensions.Logging;
using Products.Infrastructure.Interfaces.Caching;
using StackExchange.Redis;
using System.Buffers;
using System.Text.Json;

namespace Products.Infrastructure.Caching
{
    internal class CacheService<T> : ICacheService<T> where T : class
    {
        private readonly ConnectionMultiplexer? _connection;
        private readonly IDatabase? _cache;
        private readonly ILogger _loger;
        private readonly string _redisPrefix;
        private readonly int BatchSize = 1000;

        public CacheService(ILogger<CacheService<T>> loger, RedisCacheConfiguration redisCacheConfiguration)
        {
            _connection = redisCacheConfiguration?.GetConnection();
            _cache = _connection?.GetDatabase();
            _loger = loger;
            _redisPrefix = GetPrefixForType(typeof(T));

            if (_connection == null)
                _loger.LogCritical("Redis {Type} ConnectionMultiplexer null!", _redisPrefix);
        }

        public async Task<T?> GetCacheAsync(string key)
        {
            var cachedData = _cache != null ? await _cache.StringGetAsync(GetRedisKey(key)) : RedisValue.Null;

            if (cachedData.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(cachedData.ToString());
        }

        public async Task SetCacheAsync(string key, T value, TimeSpan? expiry = null)
        {
            var serializedData = JsonSerializer.Serialize(value);

            if (_cache != null)
                await _cache.StringSetAsync(GetRedisKey(key), serializedData, expiry);
        }

        public async Task InvalidateCacheAsync()
        {
            await InvalidateCacheForKeyAsync(_redisPrefix);
        }

        public async Task InvalidateCacheForTypeAsync(Type type)
        {
            var redisPrefix = GetPrefixForType(type);
            await InvalidateCacheForKeyAsync(redisPrefix);
        }

        private async Task InvalidateCacheForKeyAsync(string redisPrefix)
        {
            var keysBatch = ArrayPool<RedisKey>.Shared.Rent(BatchSize);

            if (_connection == null || _cache == null)
            {
                _loger.LogError("Cache not connected");
                return;
            }

            foreach (var endpoint in _connection.GetEndPoints())
            {
                var server = _connection.GetServer(endpoint);
                var index = 0;

                await foreach (var key in server.KeysAsync(pattern: redisPrefix + "*"))
                {
                    keysBatch[index++] = key;

                    if (index >= BatchSize)
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

        private string GetPrefixForType(Type type)
        {
            return $"{type.Namespace}.{type.Name}";
        }

        private RedisKey GetRedisKey(string key)
        {
            return new RedisKey(key).Append(_redisPrefix);
        }
    }
}
