using System.Text.Json;
using Sofc.TwitchStats.Api.Data.Configuration;
using StackExchange.Redis;

namespace Sofc.TwitchStats.Api.Service;

public class CacheService(IConnectionMultiplexer redis, JsonConfiguration jsonConfiguration) : ICacheService
{
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task SetObjectAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
#if DEBUG // Disable Cache on debug
        return;
#endif
        await _database.StringSetAsync(key, JsonSerializer.Serialize(value, jsonConfiguration.JsonSerializerOptions), expiry);
    }

    public async Task<T?> GetObjectAsync<T>(string key)
    {
#if DEBUG // Disable Cache on debug
        return default;
#endif
        var str = await _database.StringGetAsync(key);
        return str.IsNull ? default : JsonSerializer.Deserialize<T>(str.ToString(), jsonConfiguration.JsonSerializerOptions);
    }
}

public class DebugCacheService : ICacheService
{
    public Task SetObjectAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        return Task.CompletedTask;
    }

    public Task<T?> GetObjectAsync<T>(string key)
    {
        return Task.FromResult(default(T));
    }
}

public interface ICacheService
{
    Task SetObjectAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task<T?> GetObjectAsync<T>(string key);
}