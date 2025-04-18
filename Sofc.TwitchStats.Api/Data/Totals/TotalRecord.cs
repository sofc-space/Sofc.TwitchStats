using System.Text.Json.Serialization;
using Sofc.TwitchStats.Api.Data.Leetify.V2Api.Profile;

namespace Sofc.TwitchStats.Api.Data.Totals;

public record TotalRecord
{
    public int Kills { get; set; }
    public int ShadowKills { get; set; }
    public int Deaths { get; set; }
    public int ShadowDeaths { get; set; }
    public int Games { get; set; }
    public int HsKills { get; set; }
    public int TotalDamage { get; set; }
    public int RoundSum { get; set; }
    public decimal KdRate { get; set; }
    public int Win { get; set; }
    public int Loss { get; set; }
    public int Draw { get; set; }
    public decimal? HsRate { get; set; }
    public decimal WinRate { get; set; }
    public decimal? Adr { get; set; }
    public DateTimeOffset FirstMatch { get; set; }
    public DateTimeOffset? LastMatch { get; set; }
    public decimal LeetifyRatingSum { get; set; }
    public decimal LeetifyRating { get; set; }
    

    public int? PremierStart { get; set; }
    public int? PremierCurrent { get; set; }
    public int? PremierDifference { get; set; }
    public int PremierSeasonWins { get; set; }
    public int PremierSeasonDraws { get; set; }
    public int PremierSeasonLosses { get; set; }
    public decimal PremierSeasonWinRate { get; set; }

    public LeetifyTotal LeetifyTotal { get; set; }
}

public class LeetifyTotal
{
    public decimal Winrate { get; set; }
    
    public LeetifyV2Ranks Ranks { get; set; }

    public LeetifyV2Rating Rating { get; set; }

    public LeetifyV2Stats Stats { get; set; }
}
