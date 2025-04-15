using Microsoft.Extensions.Options;
using Sofc.TwitchStats.Api.Data.Configuration;
using Sofc.TwitchStats.Api.Data.Leetify;
using Sofc.TwitchStats.Api.Data.Leetify.V2Api.Match;
using Sofc.TwitchStats.Api.Data.Leetify.V2Api.Profile;
using Sofc.TwitchStats.Api.Data.Totals;

namespace Sofc.TwitchStats.Api.Service;

public class GeneratorService(LeetifyCacheService leetifyCacheService, IOptions<MetaOptions> metaOptions)
{
    public async Task<TotalRecord> GenerateStats(string steam64Id, int threshold)
    {
        var profile = await leetifyCacheService.GetProfile(steam64Id);
        var v2Profile = await leetifyCacheService.GetV2Profile(steam64Id);
        var total = new TotalRecord();

        GenerateLeetifyTotals(v2Profile, total);
        
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

    private void GenerateLeetifyTotals(LeetifyV2PlayerProfile v2Profile, TotalRecord total)
    {
        total.LeetifyTotal = new LeetifyTotal
        {
            Winrate = (decimal) v2Profile.Winrate,
            Rating = v2Profile.Rating,
            Ranks = v2Profile.Ranks,
            Stats = v2Profile.Stats,
        };

        total.LeetifyTotal.Ranks.Leetify *= 100D;
        
        total.LeetifyTotal.Rating.Clutch *= 100D;
        total.LeetifyTotal.Rating.Opening *= 100D;
        total.LeetifyTotal.Rating.CtLeetify *= 100D;
        total.LeetifyTotal.Rating.TLeetify *= 100D;
        
        total.LeetifyTotal.Stats.CtOpeningDuelSuccessPercentage /= 100D;
        total.LeetifyTotal.Stats.TOpeningDuelSuccessPercentage /= 100D;
        total.LeetifyTotal.Stats.TradedDeathsSuccessPercentage /= 100D;
        total.LeetifyTotal.Stats.TradeKillsSuccessPercentage /= 100D;
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
        if (total.RoundSum > 0)
        {
            total.Adr = (decimal) total.TotalDamage / total.RoundSum;
            total.LeetifyRating = total.LeetifyRatingSum / total.RoundSum;
            total.LeetifyRating *= 100;
        }

        total.Kills += total.ShadowKills;
        total.Deaths += total.ShadowDeaths;
        total.KdRate = (decimal)total.Kills / total.Deaths;
        
        total.PremierDifference = total.PremierCurrent - total.PremierStart;
    }

    private async Task GenerateGameStats(LeetifyListGame game, string steam64Id, TotalRecord total)
    {
        if (!Guid.TryParse(game.GameId, out var guid))
        {
            GenerateNonDetailedStats(game, total, steam64Id);
            return;
        }
        
        var matchDetail = await leetifyCacheService.GetV2Match(guid);

        GeneratePlayerStats(game, matchDetail.Stats.First(s => s.Steam64Id == steam64Id), total, steam64Id);
        
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

    private static void GeneratePlayerStats(LeetifyListGame game, LeetifyV2PlayerStats player, TotalRecord total, string steam64Id)
    {
        total.Games++;
        var kills = player.TotalKills;
        total.Kills += kills; 
        total.Deaths += player.TotalDeaths;
        total.HsKills += player.TotalHsKills;
        total.TotalDamage += player.TotalDamage;
        total.RoundSum += player.RoundsCount;
        total.LeetifyRatingSum += (decimal) player.LeetifyRating;

        if (player.RoundsWon > player.RoundsLost)
            total.Win++;
        else if (player.RoundsWon < player.RoundsLost)
            total.Loss++;
        else
            total.Draw++;
    }

    private static void GeneratePremierStats(LeetifyListGame game, TotalRecord total)
    {
        if (game.RankType != 11) return;
        total.PremierStart = game.SkillLevel;
        total.PremierCurrent ??= total.PremierStart;
    }
}