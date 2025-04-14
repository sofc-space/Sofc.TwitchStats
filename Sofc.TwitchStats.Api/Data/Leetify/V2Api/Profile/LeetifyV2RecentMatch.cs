using System.Text.Json.Serialization;

namespace Sofc.TwitchStats.Api.Data.Leetify.V2Api.Profile;

public class LeetifyV2RecentMatch
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("finished_at")]
    public DateTime FinishedAt { get; set; }
}