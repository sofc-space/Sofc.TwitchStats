namespace Sofc.TwitchStats.Configuration;

public record AppConfig
{
    public IEnumerable<AppConfigEntry> Entries { get; set; }
}

public record AppConfigEntry
{
    public string Steam64Id { get; set; }
    public string OutputPath { get; set; }
    public int Threshold { get; set; }
}