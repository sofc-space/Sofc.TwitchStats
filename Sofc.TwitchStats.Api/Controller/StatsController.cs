using Microsoft.AspNetCore.Mvc;
using Sofc.TwitchStats.Api.Data.Api;
using Sofc.TwitchStats.Api.Service;

namespace Sofc.TwitchStats.Api.Controller;

[Route("[controller]")]
[ApiController]
public class StatsController(ResultCacheService resultCacheService) : ControllerBase
{
    [HttpGet("{steam64Id}")]
    [ProducesResponseType(typeof(StatsResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StatsResult>> Get(string steam64Id, [FromQuery] int sessionDetectionThreshold = 3)
    {
        if (sessionDetectionThreshold is < 1 or > 36)
            return BadRequest("Session Detection Threshold must be between 1 and 36 hours");
        return await resultCacheService.GetStatsResult(steam64Id, sessionDetectionThreshold);
    }
}