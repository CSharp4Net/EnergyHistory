using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Controller
{
  [Route("api/[controller]")]
  public sealed class OverviewController : ControllerBase
  {
    public OverviewController(ILogger<OverviewController> logger, IDataStore dataStore)
    {
      service = new OverviewService(logger, dataStore);
    }

    private OverviewService service;

    [HttpGet]
    public IActionResult Get()
      => service.GetItems();
  }
}