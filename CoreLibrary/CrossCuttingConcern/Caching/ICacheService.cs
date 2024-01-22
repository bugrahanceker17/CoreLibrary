namespace CoreLibrary.CrossCuttingConcern.Caching;

public interface ICacheService
{
    Task Add(string key, string value);
    Task AddWithTimeSpan(string key, string value, TimeSpan expireTime);
    Task<string?> Get(string key);
    Task Remove(string key);
}