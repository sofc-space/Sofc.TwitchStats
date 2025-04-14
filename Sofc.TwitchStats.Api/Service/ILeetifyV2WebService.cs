using Refit;
using Sofc.TwitchStats.Api.Data.Leetify.V2Api;
using Sofc.TwitchStats.Api.Data.Leetify.V2Api.Match;
using Sofc.TwitchStats.Api.Data.Leetify.V2Api.Profile;

namespace Sofc.TwitchStats.Api.Service;

public interface ILeetifyV2WebService
{
    [Get("/matches/{matchId}")]
    Task<LeetifyV2Match> GetMatch(Guid matchId);
    
    [Get("/profiles/{steam64Id}")]
    Task<LeetifyV2PlayerProfile> GetProfile(string steam64Id);
}