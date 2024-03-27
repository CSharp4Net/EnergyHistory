using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter;
using CS4N.EnergyHistory.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Controller
{
  [Route("api/[controller]")]
  public sealed class ElectricMeterDefinitionController : ControllerBase
  {
    public ElectricMeterDefinitionController(ILogger logger, IDataStore dataStore)
    {
      service = new ElectricMeterDefinitionService(logger, dataStore);
    }

    private ElectricMeterDefinitionService service;

    [HttpGet("overview")]
    public IActionResult GetOverview()
      => service.GetDefinitions();

    [HttpGet("{guid}")]
    public IActionResult Get(string guid)
      => service.GetDefinition(guid);

    [HttpPost]
    public IActionResult Post([FromBody] Definition definition)
      => service.AddDefinition(definition);

    [HttpPatch]
    public IActionResult Patch([FromBody] Definition definition)
      => service.UpdateDefinition(definition);

    [HttpDelete]
    public IActionResult Delete([FromBody] Definition definition)
      => service.DeleteDefinition(definition);
  }
}