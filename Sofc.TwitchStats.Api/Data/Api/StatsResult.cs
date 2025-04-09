namespace Sofc.TwitchStats.Api.Data.Api;

public class StatsResult
{
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public decimal Kd { get; set; }
    public decimal? Adr { get; set; }
    public decimal? Hs { get; set; }
    public decimal Winrate { get; set; }
    public int PremierStart { get; set; }
    public int PremierCurrent { get; set; }
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
}