using System.Diagnostics;
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

    public async Task<LeetifyProfile> GetProfile(string profileId)
    {
        return await GetOrRequest($"profile:{profileId}", () => leetifyWebService.GetProfile(profileId),
            TimeSpan.FromMinutes(5));
    }

    public async Task<LeetifyV2Match> GetV2Match(Guid matchId)
    {
        return await GetOrRequest($"v2match:{matchId:D}", () => leetifyV2WebService.GetMatch(matchId),
            TimeSpan.FromDays(14));
    }

    public async Task<LeetifyV2PlayerProfile> GetV2Profile(string steam64Id)
    {
        return await GetOrRequest($"v2profile:{steam64Id}", () => leetifyV2WebService.GetProfile(steam64Id),
            TimeSpan.FromMinutes(5));
    }

    private async Task<T> GetOrRequest<T>(string key, Func<Task<T>> request, TimeSpan expiry) where T : class
    {
        var obj = await cacheService.GetObjectAsync<T>(key);
        if (obj != null) return obj;
        obj = await request();
        await cacheService.SetObjectAsync(key, obj, TimeSpan.FromMinutes(1));
        return obj;
    }

}