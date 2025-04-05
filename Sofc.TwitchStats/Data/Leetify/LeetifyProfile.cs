namespace Sofc.TwitchStats.Data.Leetify;

public record LeetifyProfile
{
    public IEnumerable<LeetifyListGame> Games { get; set; }
}