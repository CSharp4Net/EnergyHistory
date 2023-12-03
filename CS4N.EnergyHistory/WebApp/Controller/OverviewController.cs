using CS4N.EnergyHistory.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Controller
{
  [Route("api/[controller]")]
  public class OverviewController : ControllerBase
  {
    public OverviewController(ILogger<OverviewController> logger)
    {
      service = new OverviewService(logger);
    }

    private OverviewService service;

    [HttpGet]
    public IActionResult Get()
      => service.GetItems();
  }
}