using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.WebApp.Services;
using CS4N.EnergyHistory.WebApp.ViewModels.Station;
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

    [HttpGet("init")]
    public IActionResult GetInitData()
      => service.GetInitData();

    [HttpPost("{stationGuid}")]
    public IActionResult GetStationViewData(string stationGuid, [FromBody] ChartDataFilter filter)
      => service.GetStationViewData(stationGuid, filter);

    [HttpGet("{stationGuid}/edit")]
    public IActionResult GetStationDataForEdit(string stationGuid)
      => service.GetStationDataForEdit(stationGuid);

    [HttpGet("{stationGuid}/template/{year}")]
    public IActionResult GetStationDataTemplate(string stationGuid, int year)
      => service.GetStationDataTemplate(stationGuid, year);

    [HttpPost("{stationGuid}/edit")]
    public IActionResult PostStationDataForEdit([FromBody] StationData stationData)
      => service.PostStationDataForEdit(stationData);
  }
}