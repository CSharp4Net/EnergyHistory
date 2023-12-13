using CS4N.EnergyHistory.Contracts;
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

    //[HttpGet("{stationId}")]
    //public IActionResult GetStationData(double stationId)
    //  => service.GetStationData(stationId);
  }
}