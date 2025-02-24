namespace Products.Infrastructure.Interfaces.Caching
{
    public interface ICacheService<T> where T : class
    {
        Task SetCacheAsync(string key, IEnumerable<T> value, TimeSpan? expiry = null);

        Task<IEnumerable<T>?> GetCacheAsync(string key);

        Task InvalidateCacheAsync();
    }
}
