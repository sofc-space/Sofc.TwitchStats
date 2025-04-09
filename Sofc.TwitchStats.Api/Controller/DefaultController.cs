using Microsoft.AspNetCore.Mvc;

namespace Sofc.TwitchStats.Api.Controller;

[ApiExplorerSettings(IgnoreApi = true)]
public class DefaultController : ControllerBase
{
    [Route("/")]
    [Route("/docs")]
    [Route("/swagger")]
    public IActionResult Index()
    {
        return new RedirectResult("~/swagger");
    }
}