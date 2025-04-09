using Microsoft.Extensions.Options;
using Sofc.TwitchStats.Api.Data.Configuration;
using Sofc.TwitchStats.Api.Data.Leetify;
using Sofc.TwitchStats.Api.Data.Output;

namespace Sofc.TwitchStats.Api.Service;

public class GeneratorService(LeetifyCacheService leetifyCacheService, IOptions<MetaOptions> metaOptions)
{
    public async Task<TotalRecord> GenerateStats(string steam64Id, int threshold)
    {
        var profile = await leetifyCacheService.GetProfile(steam64Id);
        var total = new TotalRecord();
        
        var lastMatchDateTime = (DateTimeOffset?) null;
        var beginPremierFound = false;
        foreach (var game in profile.Games)
        {
            if (!beginPremierFound)
            {
                GeneratePremierStats(game, total);
            }
            
            var dateTime = game.GameFinishedAt.ToLocalTime();
            if (lastMatchDateTime != null && lastMatchDateTime.Value - dateTime > TimeSpan.FromHours(threshold))
            {
                if(game.RankType == 11)
                    beginPremierFound = true;
                
                if (dateTime > new DateTimeOffset(metaOptions.Value.CurrentPremierSeasonStart).AddMinutes(30))
                {
                    AddPremierSeasonStats(game, total);
                    continue;
                }
                break;
            }
            
            lastMatchDateTime = total.FirstMatch = dateTime;
            total.LastMatch ??= dateTime;
            AddPremierSeasonStats(game, total);

            await GenerateGameStats(game, steam64Id, total);
        }

        GenerateTotalStats(total);

        return total;
    }

    private static void AddPremierSeasonStats(LeetifyListGame game, TotalRecord total)
    {
        if (game.RankType == 11)
        {
            switch (game.MatchResult)
            {
                case LeetifyMatchResult.Win:
                    total.PremierSeasonWins++;
                    break;
                case LeetifyMatchResult.Loss:
                    total.PremierSeasonLosses++;
                    break;
                case LeetifyMatchResult.Tie:
                    total.PremierSeasonDraws++;
                    break;
            }
        }
    }

    private static void GenerateTotalStats(TotalRecord total)
    {
        if (total.Games <= 0) return;
        if(total.Kills > 0)
            total.HsRate = (decimal)total.HsKills / total.Kills;
        total.WinRate = (decimal) total.Win / (total.Win + total.Draw + total.Loss);
        total.PremierSeasonWinRate = (decimal) total.PremierSeasonWins / (total.PremierSeasonWins + total.PremierSeasonDraws + total.PremierSeasonLosses);
        if(total.RoundSum > 0)
            total.Adr = (decimal) total.TotalDamage / total.RoundSum;

        total.Kills += total.ShadowKills;
        total.Deaths += total.ShadowDeaths;
        total.KdRate = (decimal)total.Kills / total.Deaths;
    }

    private async Task GenerateGameStats(LeetifyListGame game, string steam64Id, TotalRecord total)
    {
        if (!Guid.TryParse(game.GameId, out var guid))
        {
            GenerateNonDetailedStats(game, total, steam64Id);
            return;
        }
        
        var gameDetail = await leetifyCacheService.GetGame(guid);

        if (gameDetail.Status == LeetifyGameStatus.Error && gameDetail.GamePlayerRoundSkeletonStats.Any())
        {
            GenerateSkeletonStats(gameDetail.GamePlayerRoundSkeletonStats.First(s => s.Steam64Id == steam64Id), total);
        }

        if (gameDetail.Status == LeetifyGameStatus.Ready)
        {
            GeneratePlayerStats(game, gameDetail.PlayerStats.First(s => s.Steam64Id == steam64Id), total, steam64Id);
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


    private void GenerateNonDetailedStats(LeetifyListGame game, TotalRecord total, string steam64Id)
    {
        total.Games++;
        total.ShadowKills += game.Kills;
        total.ShadowDeaths += game.Deaths;
        
        //GenerateRounds(game, steam64Id, total);
        
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

    private static void GeneratePlayerStats(LeetifyListGame game, LeetifyGamePlayerStat stat, TotalRecord total, string steam64Id)
    {
        total.Games++;
        var kills = stat.TotalKills;
        total.Kills += kills; 
        total.Deaths += stat.TotalDeaths;
        total.HsKills += (int) Math.Round(stat.Hsp * kills);
        total.TotalDamage += stat.TotalDamage;
        GenerateRounds(game, steam64Id, total);
        
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

    private static void GenerateRounds(LeetifyListGame game, string steam64Id, TotalRecord total)
    {
        total.RoundSum += game.OwnTeamTotalLeetifyRatingRounds.First(r => r.Key == steam64Id)!.Value;
    }

    private static void GeneratePremierStats(LeetifyListGame game, TotalRecord total)
    {
        if (game.RankType != 11) return;
        total.PremierStart = game.SkillLevel;
        total.PremierCurrent ??= total.PremierStart;
    }
}