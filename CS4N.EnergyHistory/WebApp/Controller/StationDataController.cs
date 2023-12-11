using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;
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

    [HttpGet("{stationId}/{year}/{month}")]
    public IActionResult GetMonth(int stationId, int year, int month)
      => service.GetStationDataOfMonth(stationId, year, month);

    [HttpGet("{stationId}/{year}")]
    public IActionResult GetYear(int stationId, int year)
      => service.GetStationDataOfYear(stationId, year);

    [HttpPost]
    public IActionResult Post([FromBody] StationDataMonth data)
      => service.AddDataOfMonth(data);

    [HttpPatch]
    public IActionResult Patch([FromBody] StationDataMonth data)
      => service.UpdateDataOfMonth(data);

    [HttpDelete]
    public IActionResult Delete([FromBody] StationDataMonth data)
      => service.DeleteDataOfMonth(data);
  }
}