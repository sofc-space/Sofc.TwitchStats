namespace Sofc.TwitchStats.Api.Data.Leetify;

public record LeetifyProfile
{
    public IEnumerable<LeetifyListGame> Games { get; set; }
}