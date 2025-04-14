using System.Text.Json.Serialization;

namespace Sofc.TwitchStats.Api.Data.Leetify.V2Api.Match;

public class LeetifyV2Match
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("replay_url")]
    public string ReplayUrl { get; set; }

    [JsonPropertyName("finished_at")]
    public DateTimeOffset FinishedAt { get; set; }

    [JsonPropertyName("data_source")]
    public string DataSource { get; set; }

    [JsonPropertyName("data_source_match_id")]
    public string DataSourceMatchId { get; set; }

    [JsonPropertyName("map_name")]
    public string MapName { get; set; }

    [JsonPropertyName("has_banned_player")]
    public bool HasBannedPlayer { get; set; }

    [JsonPropertyName("team_scores")]
    public List<LeetifyV2TeamScore> TeamScores { get; set; }

    [JsonPropertyName("stats")]
    public List<LeetifyV2PlayerStats> Stats { get; set; }
}