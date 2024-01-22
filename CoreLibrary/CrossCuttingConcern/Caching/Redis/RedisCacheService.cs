using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;

namespace CoreLibrary.CrossCuttingConcern.Caching.Redis;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development ? "DEV" : "PROD";

    public RedisCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }
    
    public async Task Add(string key, string value)
    {
        await _distributedCache.SetStringAsync($"{env}_{key}", value);
    }

    public async Task AddWithTimeSpan(string key, string value, TimeSpan expireTime)
    {
        await _distributedCache.SetStringAsync($"{env}_{key}", value, new DistributedCacheEntryOptions().SetSlidingExpiration(expireTime));
    }

    public async Task<string?> Get(string key)
    {
        return await _distributedCache.GetStringAsync($"{env}_{key}");
    }

    public async Task Remove(string key)
    {
        await _distributedCache.RemoveAsync($"{env}_{key}");
    }
}