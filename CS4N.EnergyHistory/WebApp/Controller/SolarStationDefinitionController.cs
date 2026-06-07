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
    public IActionResult GetDefinitions()
      => service.GetDefinitions();

    [HttpGet("{guid}")]
    public IActionResult GetDefinition(string guid)
      => service.GetDefinition(guid);

    [HttpPost]
    public IActionResult Post([FromBody] SolarStationDefinition definition)
      => service.AddDefinition(definition);

    [HttpPatch]
    public IActionResult Patch([FromBody] SolarStationDefinition definition)
      => service.UpdateDefinition(definition);

    [HttpDelete]
    public IActionResult Delete([FromBody] SolarStationDefinition definition)
      => service.DeleteDefinition(definition);
  }
}