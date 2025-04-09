using Sofc.TwitchStats.Api.Data.Leetify;

namespace Sofc.TwitchStats.Api.Service;

public class LeetifyCacheService(ILeetifyWebService leetifyWebService, ILogger<LeetifyCacheService> logger)
{
    
    private readonly IDictionary<Guid, LeetifyGame> _games = new Dictionary<Guid, LeetifyGame>();
    private readonly IDictionary<string, CachedProfile> _profiles = new Dictionary<string, CachedProfile>();

    public async Task<LeetifyGame> GetGame(Guid gameId)
    {
        if (_games.TryGetValue(gameId, out var game1)) return game1;
        
        var game = await leetifyWebService.GetGame(gameId);
        _games.Add(gameId, game);
        return game;
    }

    public async Task<LeetifyProfile> GetProfile(string profileId)
    {
        if (_profiles.TryGetValue(profileId, out var profile) && profile.CreatedAt > DateTimeOffset.Now.AddMinutes(-1)) return profile.Profile;
        try
        {
            var leetifyProfile = await leetifyWebService.GetProfile(profileId);
            var cached = new CachedProfile
            {
                CreatedAt = DateTimeOffset.Now,
                Profile = leetifyProfile,
            };
            _profiles.Add(profileId, cached);
            return leetifyProfile;
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            if (profile != null)
                return profile.Profile;
            throw;
        }
    }
    
    class CachedProfile
    {
        public DateTimeOffset CreatedAt { get; set; }
        public LeetifyProfile Profile { get; set; }
    }
}