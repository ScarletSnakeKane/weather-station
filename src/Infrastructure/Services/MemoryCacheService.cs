using Core.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<T?> GetAsync<T>(string key)
        {
            _cache.TryGetValue(key, out T? value);
            return Task.FromResult(value);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan ttl)
        {
            _cache.Set(key, value, ttl);
            return Task.CompletedTask;
        }
    }
}

