using System.Text.Json;
using StackExchange.Redis;

namespace Sofc.TwitchStats.Api.Service;

public class CacheService(IConnectionMultiplexer redis)
{
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task SetObjectAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        await _database.StringSetAsync(key, JsonSerializer.Serialize(value), expiry);
    }

    public async Task<T?> GetObjectAsync<T>(string key)
    {
        var str = await _database.StringGetAsync(key);
        return str.IsNull ? default : JsonSerializer.Deserialize<T>(str.ToString());
    }
}