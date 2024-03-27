using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.SolarStation;
using CS4N.EnergyHistory.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Controller
{
  [Route("api/[controller]")]
  public sealed class SolarStationDefinitionController : ControllerBase
  {
    public SolarStationDefinitionController(ILogger logger, IDataStore dataStore)
    {
      service = new SolarStationDefinitionService(logger, dataStore);
    }

    private SolarStationDefinitionService service;

    [HttpGet("overview")]
    public IActionResult GetList()
      => service.GetStations();

    [HttpGet("{guid}")]
    public IActionResult Get(string guid)
      => service.GetStation(guid);

    [HttpPost]
    public IActionResult Post([FromBody] Definition station)
      => service.AddStation(station);

    [HttpPatch]
    public IActionResult Patch([FromBody] Definition station)
      => service.UpdateStation(station);

    [HttpDelete]
    public IActionResult Delete([FromBody] Definition station)
      => service.DeleteStation(station);
  }
}