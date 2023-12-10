using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Controller
{
  [Route("api/[controller]")]
  public sealed class CockpitController : ControllerBase
  {
    public CockpitController(ILogger<CockpitController> logger, IDataStore dataStore)
    {
      service = new CockpitService(logger, dataStore);
    }

    private CockpitService service;

    [HttpGet]
    public IActionResult Get()
      => service.GetItems();
  }
}