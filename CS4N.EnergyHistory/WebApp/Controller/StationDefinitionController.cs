using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.Definition;
using CS4N.EnergyHistory.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Controller
{
  [Route("api/[controller]")]
  public sealed class StationDefinitionController : ControllerBase
  {
    public StationDefinitionController(ILogger<StationDefinitionController> logger, IDataStore dataStore)
    {
      service = new StationDefinitionService(logger, dataStore);
    }

    private StationDefinitionService service;

    [HttpGet("overview")]
    public IActionResult GetList()
      => service.GetStations();

    [HttpGet("{id}")]
    public IActionResult Get(double id)
      => service.GetStation(id);

    [HttpPost]
    public IActionResult Post([FromBody] StationDefinition station)
      => service.AddStation(station);

    [HttpPatch]
    public IActionResult Patch([FromBody] StationDefinition station)
      => service.UpdateStation(station);

    [HttpDelete]
    public IActionResult Delete([FromBody] StationDefinition station)
      => service.DeleteStation(station);
  }
}