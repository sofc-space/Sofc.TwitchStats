using Refit;
using Sofc.TwitchStats.Data.Leetify;

namespace Sofc.TwitchStats.Service;

public interface ILeetifyWebService
{
    [Get("/profile/id/{steam64Id}")]
    Task<LeetifyProfile> GetProfile(string steam64Id);

    [Get("/games/{gameId}")]
    Task<LeetifyGame> GetGame(Guid gameId);
}