namespace Sofc.TwitchStats.Data.Leetify;

public record LeetifyListGame
{
    public int? RankType { get; set; }
    public int SkillLevel { get; set; }
    public Guid GameId { get; set; }
    public DateTimeOffset GameFinishedAt { get; set; }
    public LeetifyMatchResult MatchResult { get; set; }
}