using System.Text.Json.Serialization;

namespace Sofc.TwitchStats.Api.Data.Leetify.V2Api.Profile;

public class LeetifyV2Rating
{
    [JsonPropertyName("aim")]
    public double Aim { get; set; }

    [JsonPropertyName("positioning")]
    public double Positioning { get; set; }

    [JsonPropertyName("utility")]
    public double Utility { get; set; }

    [JsonPropertyName("clutch")]
    public double Clutch { get; set; }

    [JsonPropertyName("opening")]
    public double Opening { get; set; }

    [JsonPropertyName("ct_leetify")]
    public double CtLeetify { get; set; }

    [JsonPropertyName("t_leetify")]
    public double TLeetify { get; set; }
}