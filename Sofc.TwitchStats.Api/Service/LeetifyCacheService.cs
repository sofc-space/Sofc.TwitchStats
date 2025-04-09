using System.Text.Json;
using System.Text.Json.Serialization;
using Sofc.TwitchStats.Api.Data.Leetify;
using StackExchange.Redis;

namespace Sofc.TwitchStats.Api.Service;

public class LeetifyCacheService
{
    
    private readonly ILeetifyWebService _leetifyWebService;
    private readonly ILogger<LeetifyCacheService> _logger;
    private readonly IDatabase _database;

    public LeetifyCacheService(ILeetifyWebService leetifyWebService, ILogger<LeetifyCacheService> logger, IConnectionMultiplexer connectionMultiplexer)
    {
        _leetifyWebService = leetifyWebService;
        _logger = logger;
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task<LeetifyGame> GetGame(Guid gameId)
    {
        var key = $"game:{gameId:D}";
        var game = await GetObject<LeetifyGame>(key);
        if (game != null) return game;
        game = await _leetifyWebService.GetGame(gameId);
        await SetObject(key, game);
        return game;
    }

    public async Task<LeetifyProfile> GetProfile(string profileId)
    {
        var key = $"profile:{profileId}";
        var profile = await GetObject<LeetifyProfile>(key);
        if (profile != null) return profile;
        var leetifyProfile = await _leetifyWebService.GetProfile(profileId);
        await SetObject(key, leetifyProfile, TimeSpan.FromMinutes(1));
        return leetifyProfile;
    }

    private async Task SetObject<T>(string key, T value, TimeSpan? expiry = null)
    {
        await _database.StringSetAsync(key, JsonSerializer.Serialize(value), expiry);
    }

    private async Task<T?> GetObject<T>(string key)
    {
        var str = await _database.StringGetAsync(key);
        return str.IsNull ? default : JsonSerializer.Deserialize<T>(str.ToString());
    }
}