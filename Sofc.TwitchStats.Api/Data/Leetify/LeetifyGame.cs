namespace Sofc.TwitchStats.Api.Data.Leetify;

public record LeetifyGame
{
    public LeetifyGameStatus Status { get; set; }
    public IEnumerable<LeetifyGameSkeletonStat> GamePlayerRoundSkeletonStats { get; set; }
    public IEnumerable<LeetifyGamePlayerStat> PlayerStats { get; set; }
    public IEnumerable<int> TeamScores { get; set; }
}