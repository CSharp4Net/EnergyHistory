using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Controller
{
  [Route("api/[controller]")]
  public sealed class StationDataController : ControllerBase
  {
    public StationDataController(ILogger logger, IDataStore dataStore)
    {
      service = new StationDataService(logger, dataStore);
    }

    private StationDataService service;

    [HttpGet("{stationId}/kpi")]
    public IActionResult GetKpiValue(string stationId)
      => service.GetKpiValue(stationId);

    [HttpGet("{stationId}/{year}/{month}")]
    public IActionResult GetStationViewData(string stationId, int year, int month)
      => service.GetStationViewData(stationId, year, month);

    [HttpGet("{stationId}")]
    public IActionResult GetStationDataForEdit(string stationId)
      => service.GetStationDataForEdit(stationId);

    [HttpGet("template/{year}")]
    public IActionResult GetStationDataTemplate(int year)
      => service.GetStationDataTemplate(year);

    [HttpPost]
    public IActionResult PostStationDataForEdit([FromBody] StationData stationData)
      => service.PostStationDataForEdit(stationData);
  }
}