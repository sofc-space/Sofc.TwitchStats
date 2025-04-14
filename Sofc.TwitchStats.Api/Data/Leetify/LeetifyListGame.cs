namespace Sofc.TwitchStats.Api.Data.Leetify;

public record LeetifyListGame
{
    public int? RankType { get; set; }
    public int? SkillLevel { get; set; }
    public string GameId { get; set; }
    public DateTimeOffset GameFinishedAt { get; set; }
    public LeetifyMatchResult MatchResult { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
}