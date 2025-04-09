namespace Sofc.TwitchStats.Api.Data.Leetify;

public record LeetifyGamePlayerStat
{
    public string Steam64Id { get; set; }
    public int TotalKills { get; set; }
    public int TotalDeaths { get; set; }
    public double Hsp { get; set; }
    public int TotalDamage { get; set; }
}