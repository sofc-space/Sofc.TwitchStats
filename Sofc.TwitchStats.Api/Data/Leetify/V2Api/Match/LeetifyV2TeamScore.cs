using System.Text.Json.Serialization;

namespace Sofc.TwitchStats.Api.Data.Leetify.V2Api.Match;

public class LeetifyV2TeamScore
{
    [JsonPropertyName("team_number")]
    public int TeamNumber { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }
}