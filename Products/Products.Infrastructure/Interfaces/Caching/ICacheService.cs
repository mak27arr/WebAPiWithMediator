namespace Products.Infrastructure.Interfaces.Caching
{
    public interface ICacheService<T> where T : class
    {
        Task SetCacheAsync(string key, T value, TimeSpan? expiry = null);

        Task<T> GetCacheAsync(string key);

        Task InvalidateCacheAsync();

        Task InvalidateCacheForTypeAsync(Type type);
    }
}
