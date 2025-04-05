using Sofc.TwitchStats.Configuration;
using Sofc.TwitchStats.Data.Leetify;
using Sofc.TwitchStats.Data.Output;

namespace Sofc.TwitchStats.Service;

public class GeneratorService(ILeetifyWebService leetifyWebService)
{
    public async Task<TotalRecord> GenerateStats(AppConfigEntry config)
    {
        var profile = await leetifyWebService.GetProfile(config.Steam64Id);
        var total = new TotalRecord();
        
        var lastMatchDateTime = (DateTimeOffset?) null;
        foreach (var game in profile.Games)
        {
            
            GeneratePremierStats(game, total);
            
            var dateTime = game.GameFinishedAt.ToLocalTime();
            if (lastMatchDateTime != null && lastMatchDateTime.Value - dateTime > TimeSpan.FromHours(config.Threshold))
            {
                if(game.RankType != 11)
                    continue;
                break;
            }
            
            lastMatchDateTime = dateTime;

            await GenerateGameStats(game, config, total);
        }

        GenerateTotalStats(total);

        return total;
    }

    private static void GenerateTotalStats(TotalRecord total)
    {
        if (total.Games <= 0) return;
        total.HsRate = (decimal)total.HsKills / total.Kills;
        total.WinRate = (decimal) total.Win / (total.Win + total.Loss);
        total.Adr = (double) total.TotalDamage / total.RoundSum;

        total.Kills += total.ShadowKills;
        total.Deaths += total.ShadowDeaths;
        total.KdRate = (double)total.Kills / total.Deaths;
    }

    private async Task GenerateGameStats(LeetifyListGame game, AppConfigEntry config, TotalRecord total)
    {
        var gameDetail = await leetifyWebService.GetGame(game.GameId);

        if (gameDetail.Status == LeetifyGameStatus.Error && gameDetail.GamePlayerRoundSkeletonStats.Any())
        {
            GenerateSkeletonStats(gameDetail.GamePlayerRoundSkeletonStats.First(s => s.Steam64Id == config.Steam64Id), total);
        }

        if (gameDetail.Status == LeetifyGameStatus.Ready)
        {
            GeneratePlayerStats(game, gameDetail, gameDetail.PlayerStats.First(s => s.Steam64Id == config.Steam64Id), total);
        }
        
    }

    private static void GeneratePlayerStats(LeetifyListGame game, LeetifyGame detailGame, LeetifyGamePlayerStat stat, TotalRecord total)
    {
        total.Games++;
        var kills = stat.TotalKills;
        total.Kills += kills; 
        total.Deaths += stat.TotalDeaths;
        total.HsKills += (int) Math.Round(stat.Hsp * kills);
        total.TotalDamage += stat.TotalDamage;
        total.RoundSum += detailGame.TeamScores.Sum();
        
        switch (game.MatchResult)
        {
            case LeetifyMatchResult.Win:
                total.Win++;
                break;
            case LeetifyMatchResult.Loss:
                total.Loss++;
                break;
            case LeetifyMatchResult.Tie:
                total.Draw++;
                break;
        }
    }

    private static void GenerateSkeletonStats(LeetifyGameSkeletonStat stat, TotalRecord total)
    {
        total.Games++;
        total.ShadowKills += stat.Kills;
        total.ShadowDeaths += stat.Deaths;
        if (stat.IsWon)
            total.Win++;
        else
            total.Loss++;
    }
    
    private static void GeneratePremierStats(LeetifyListGame game, TotalRecord total)
    {
        if (game.RankType != 11) return;
        total.PremierStart = game.SkillLevel;
        total.PremierCurrent ??= total.PremierStart;
    }
}