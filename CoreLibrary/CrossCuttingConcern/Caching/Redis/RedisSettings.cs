using Microsoft.Extensions.DependencyInjection;

namespace CoreLibrary.CrossCuttingConcern.Caching.Redis;

public static class RedisSettings
{
    public static void Initialize(this IServiceCollection services, string connectionString)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
        });
    }
}