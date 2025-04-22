using System.Text.Json.Serialization;

namespace Sofc.TwitchStats.Api.Data.Leetify.V2Api.Profile;

public class LeetifyV2RanksCompetitive
{
    [JsonPropertyName("map_name")]
    public string MapName { get; set; }
    
    [JsonPropertyName("rank")]
    public int? Rank { get; set; }
}