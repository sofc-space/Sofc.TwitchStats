using Microsoft.AspNetCore.Mvc;
using Sofc.TwitchStats.Api.Data.Api;
using Sofc.TwitchStats.Api.Data.Output;
using Sofc.TwitchStats.Api.Service;

namespace Sofc.TwitchStats.Api.Controller;

[Route("[controller]")]
[ApiController]
public class StatsController(GeneratorService generatorService) : ControllerBase
{
    [HttpGet("{steam64Id}")]
    public async Task<StatsResult> Get(string steam64Id)
    {
        var stats = await generatorService.GenerateStats(steam64Id, 3);

        return new StatsResult
        {
            Kills = stats.Kills,
            Deaths = stats.Deaths,
            Kd = stats.KdRate,
            Adr = stats.Adr,
            Hs = stats.HsRate,
            Winrate = stats.WinRate,
            PremierStart = stats.PremierStart!.Value,
            PremierCurrent = stats.PremierCurrent!.Value,
            Matches = stats.Games,
            Wins = stats.Win,
            Draws = stats.Draw,
            Losses = stats.Loss,
            FirstMatch = stats.FirstMatch,
            LastMatch = stats.LastMatch!.Value,
            PremierSeasonWins = stats.PremierSeasonWins,
            PremierSeasonDraws = stats.PremierSeasonDraws,
            PremierSeasonLosses = stats.PremierSeasonLosses,
            PremierSeasonWinRate = stats.PremierSeasonWinRate,
        };
    }
}