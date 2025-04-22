using System.Text.Json.Serialization;

namespace Sofc.TwitchStats.Api.Data.Leetify.V2Api.Profile;

public class LeetifyV2Ranks
{
    [JsonPropertyName("leetify")]
    public double Leetify { get; set; }

    [JsonPropertyName("premier")]
    public int? Premier { get; set; }

    [JsonPropertyName("faceit")]
    public int? Faceit { get; set; }

    [JsonPropertyName("faceit_elo")]
    public int? FaceitElo { get; set; }
    
    [JsonPropertyName("wingman")]
    public int? Wingman { get; set; }
    
    [JsonPropertyName("competitive")]
    public IEnumerable<LeetifyV2RanksCompetitive> Competitive { get; set; }
}