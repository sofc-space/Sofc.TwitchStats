using System.Text.Json.Serialization;

namespace Sofc.TwitchStats.Api.Data.Leetify.V2Api.Profile;

public class LeetifyV2RecentTeammate
{
    [JsonPropertyName("steam64_id")]
    public string Steam64Id { get; set; }

    [JsonPropertyName("recent_matches_count")]
    public int RecentMatchesCount { get; set; }
}