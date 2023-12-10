using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Controller
{
  [Route("api/[controller]")]
  public sealed class StationDataController : ControllerBase
  {
    public StationDataController(ILogger<StationDataController> logger, IDataStore dataStore)
    {
      service = new StationDataService(logger, dataStore);
    }

    private StationDataService service;

    [HttpGet("{id}")]
    public IActionResult Get(int id)
      => service.GetStation(id);
  }
}