using Sofc.TwitchStats.Api.Data.Leetify;

namespace Sofc.TwitchStats.Api.Service;

public class LeetifyCacheService(
    ILeetifyWebService leetifyWebService,
    ILogger<LeetifyCacheService> logger,
    CacheService cacheService)
{
    private readonly ILogger<LeetifyCacheService> _logger = logger;

    public async Task<LeetifyGame> GetGame(Guid gameId)
    {
        var key = $"game:{gameId:D}";
        var game = await cacheService.GetObjectAsync<LeetifyGame>(key);
        if (game != null) return game;
        game = await leetifyWebService.GetGame(gameId);
        await cacheService.SetObjectAsync(key, game);
        return game;
    }

    public async Task<LeetifyProfile> GetProfile(string profileId)
    {
        var key = $"profile:{profileId}";
        var profile = await cacheService.GetObjectAsync<LeetifyProfile>(key);
        if (profile != null) return profile;
        var leetifyProfile = await leetifyWebService.GetProfile(profileId);
        await cacheService.SetObjectAsync(key, leetifyProfile, TimeSpan.FromMinutes(1));
        return leetifyProfile;
    }

}