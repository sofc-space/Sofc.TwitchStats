using Microsoft.AspNetCore.Mvc;
using Sofc.TwitchStats.Api.Data.Api;
using Sofc.TwitchStats.Api.Data.Output;
using Sofc.TwitchStats.Api.Service;

namespace Sofc.TwitchStats.Api.Controller;

[Route("[controller]")]
[ApiController]
public class StatsController(ResultCacheService resultCacheService) : ControllerBase
{
    [HttpGet("{steam64Id}")]
    public async Task<StatsResult> Get(string steam64Id)
    {
        return await resultCacheService.GetStatsResult(steam64Id);
    }
}