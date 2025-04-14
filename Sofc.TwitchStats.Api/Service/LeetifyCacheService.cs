using Sofc.TwitchStats.Api.Data.Leetify;
using Sofc.TwitchStats.Api.Data.Leetify.V2Api.Match;
using Sofc.TwitchStats.Api.Data.Leetify.V2Api.Profile;

namespace Sofc.TwitchStats.Api.Service;

public class LeetifyCacheService(
    ILeetifyWebService leetifyWebService,
    ILogger<LeetifyCacheService> logger,
    CacheService cacheService,
    ILeetifyV2WebService leetifyV2WebService)
{
    private readonly ILogger<LeetifyCacheService> _logger = logger;

    public async Task<LeetifyGame> GetGame(Guid gameId)
    {
        var key = $"game:{gameId:D}";
        var game = await cacheService.GetObjectAsync<LeetifyGame>(key);
        if (game != null) return game;
        game = await leetifyWebService.GetGame(gameId);
        await cacheService.SetObjectAsync(key, game, TimeSpan.FromDays(14));
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

    public async Task<LeetifyV2Match> GetV2Match(Guid matchId)
    {
        var key = $"v2match:{matchId:D}";
        var game = await cacheService.GetObjectAsync<LeetifyV2Match>(key);
        if (game != null) return game;
        game = await leetifyV2WebService.GetMatch(matchId);
        await cacheService.SetObjectAsync(key, game, TimeSpan.FromDays(14));
        return game;
    }

    public async Task<LeetifyV2PlayerProfile> GetV2Profile(string steam64Id)
    {
        var key = $"v2profile:{steam64Id}";
        var profile = await cacheService.GetObjectAsync<LeetifyV2PlayerProfile>(key);
        if (profile != null) return profile;
        var leetifyProfile = await leetifyV2WebService.GetProfile(steam64Id);
        await cacheService.SetObjectAsync(key, leetifyProfile, TimeSpan.FromMinutes(1));
        return leetifyProfile;
    }

}