using Refit;
using Sofc.TwitchStats.Api.Data.Leetify;

namespace Sofc.TwitchStats.Api.Service;

public interface ILeetifyWebService
{
    [Get("/profile/id/{steam64Id}")]
    Task<LeetifyProfile> GetProfile(string steam64Id);

    [Get("/games/{gameId}")]
    Task<LeetifyGame> GetGame(Guid gameId);
}