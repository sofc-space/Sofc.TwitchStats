using Sofc.TwitchStats.Api.Data.Api;

namespace Sofc.TwitchStats.Api.Service;

public class ResultCacheService(GeneratorService generatorService, CacheService cacheService)
{
    public async Task<StatsResult> GetStatsResult(string steam64Id)
    {
        var key = $"StatsResult:{steam64Id}";
        var stats = await cacheService.GetObjectAsync<StatsResult>(key);

        if (stats != null) return stats;
        
        var generatorStats = await generatorService.GenerateStats(steam64Id, 3);

        stats = new StatsResult
        {
            Kills = generatorStats.Kills,
            Deaths = generatorStats.Deaths,
            Kd = generatorStats.KdRate,
            Adr = generatorStats.Adr,
            Hs = generatorStats.HsRate,
            Winrate = generatorStats.WinRate,
            PremierStart = generatorStats.PremierStart!.Value,
            PremierCurrent = generatorStats.PremierCurrent!.Value,
            Matches = generatorStats.Games,
            Wins = generatorStats.Win,
            Draws = generatorStats.Draw,
            Losses = generatorStats.Loss,
            FirstMatch = generatorStats.FirstMatch,
            LastMatch = generatorStats.LastMatch!.Value,
            PremierSeasonWins = generatorStats.PremierSeasonWins,
            PremierSeasonDraws = generatorStats.PremierSeasonDraws,
            PremierSeasonLosses = generatorStats.PremierSeasonLosses,
            PremierSeasonWinRate = generatorStats.PremierSeasonWinRate,
        };
        await cacheService.SetObjectAsync(key, stats, TimeSpan.FromMinutes(1));
        return stats;
    }
}