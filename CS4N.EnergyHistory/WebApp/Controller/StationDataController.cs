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

    [HttpGet("{stationGuid}/kpi")]
    public IActionResult GetKpiValue(string stationGuid)
      => service.GetKpiValue(stationGuid);

    [HttpGet("{stationGuid}/{year}/{month}")]
    public IActionResult GetStationViewData(string stationGuid, int year, int month)
      => service.GetStationViewData(stationGuid, year, month);

    [HttpGet("{stationGuid}")]
    public IActionResult GetStationDataForEdit(string stationGuid)
      => service.GetStationDataForEdit(stationGuid);

    [HttpGet("template/{year}")]
    public IActionResult GetStationDataTemplate(int year)
      => service.GetStationDataTemplate(year);

    [HttpPost]
    public IActionResult PostStationDataForEdit([FromBody] StationData stationData)
      => service.PostStationDataForEdit(stationData);
  }
}