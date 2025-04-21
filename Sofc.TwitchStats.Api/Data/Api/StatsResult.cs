using Sofc.TwitchStats.Api.Data.Leetify.V2Api.Profile;

namespace Sofc.TwitchStats.Api.Data.Api;

public class  StatsResult
{
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public decimal Kd { get; set; }
    public decimal? Adr { get; set; }
    public decimal? Hs { get; set; }
    public decimal Winrate { get; set; }
    public int PremierStart { get; set; }
    public int PremierCurrent { get; set; }
    public int PremierDifference { get; set; }
    public int Matches { get; set; }
    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    public DateTimeOffset FirstMatch { get; set; }
    public DateTimeOffset LastMatch { get; set; }
    public int PremierSeasonWins { get; set; }
    public int PremierSeasonDraws { get; set; }
    public int PremierSeasonLosses { get; set; }
    public decimal PremierSeasonWinRate { get; set; }
    public decimal LeetifyRating { get; set; }
    public LeetifyStatsResult Overall { get; set; }
}

public class LeetifyStatsResult
{
    public decimal Winrate { get; set; }
    
    public LeetifyRanksStatsResult Ranks { get; set; }

    public LeetifyRatingStatsResult Rating { get; set; }

    public LeetifyStatsStatsResult Stats { get; set; }
}


public class LeetifyRanksStatsResult
{
    public double Leetify { get; set; }
    public int Premier { get; set; }
    public int Faceit { get; set; }
    public int FaceitElo { get; set; }
}

public class LeetifyRatingStatsResult
{
    public double Aim { get; set; }
    public double Positioning { get; set; }
    public double Utility { get; set; }
    public double Clutch { get; set; }
    public double Opening { get; set; }
    public double CtLeetify { get; set; }
    public double TLeetify { get; set; }
}


public class LeetifyStatsStatsResult
{
    public double AccuracyEnemySpotted { get; set; }
    public double AccuracyHead { get; set; }
    public double CounterStrafingGoodShotsRatio { get; set; }
    public double CtOpeningAggressionSuccessRate { get; set; }
    public double CtOpeningDuelSuccessPercentage { get; set; }
    public double FlashbangHitFoeAvgDuration { get; set; }
    public double FlashbangHitFoePerFlashbang { get; set; }
    public double FlashbangHitFriendPerFlashbang { get; set; }
    public double FlashbangLeadingToKill { get; set; }
    public double FlashbangThrown { get; set; }
    public double HeFoesDamageAvg { get; set; }
    public double HeFriendsDamageAvg { get; set; }
    public double Preaim { get; set; }
    public double ReactionTime { get; set; }
    public double SprayAccuracy { get; set; }
    public double TOpeningAggressionSuccessRate { get; set; }
    public double TOpeningDuelSuccessPercentage { get; set; }
    public double TradedDeathsSuccessPercentage { get; set; }
    public double TradeKillOpportunitiesPerRound { get; set; }
    public double TradeKillsSuccessPercentage { get; set; }
    public double UtilityOnDeathAvg { get; set; }
}