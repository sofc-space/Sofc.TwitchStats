using System.Text.Json.Serialization;

namespace Sofc.TwitchStats.Api.Data.Leetify.V2Api.Profile;


public class LeetifyV2PlayerProfile
{
    [JsonPropertyName("winrate")]
    public double Winrate { get; set; }

    [JsonPropertyName("matchmaking_wins")]
    public int MatchmakingWins { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("bans")]
    public List<string> Bans { get; set; }

    [JsonPropertyName("steam64_id")]
    public string Steam64Id { get; set; }

    [JsonPropertyName("ranks")]
    public LeetifyV2Ranks Ranks { get; set; }

    [JsonPropertyName("rating")]
    public LeetifyV2Rating Rating { get; set; }

    [JsonPropertyName("stats")]
    public LeetifyV2Stats Stats { get; set; }

    [JsonPropertyName("recent_matches")]
    public List<LeetifyV2RecentMatch> RecentMatches { get; set; }

    [JsonPropertyName("recent_teammates")]
    public List<LeetifyV2RecentTeammate> RecentTeammates { get; set; }
}