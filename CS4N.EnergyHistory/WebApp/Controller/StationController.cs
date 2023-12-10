using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Controller
{
  [Route("api/[controller]")]
  public sealed class StationController : ControllerBase
  {
    public StationController(ILogger<StationController> logger, IDataStore dataStore)
    {
      service = new StationService(logger, dataStore);
    }

    private StationService service;

    [HttpGet("list")]
    public IActionResult GetList()
      => service.GetStations();

    [HttpGet("{id}")]
    public IActionResult Get(int id)
      => service.GetStation(id);

    [HttpPost]
    public IActionResult Post([FromBody] Station station)
      => service.AddStation(station);

    [HttpPatch]
    public IActionResult Patch([FromBody] Station station)
      => service.UpdateStation(station);

    [HttpDelete]
    public IActionResult Delete([FromBody] Station station)
      => service.DeleteStation(station);
  }
}