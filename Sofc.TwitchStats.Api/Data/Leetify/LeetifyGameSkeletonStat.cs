namespace Sofc.TwitchStats.Api.Data.Leetify;

public record LeetifyGameSkeletonStat
{
    public string Steam64Id { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public bool IsWon { get; set; }
}