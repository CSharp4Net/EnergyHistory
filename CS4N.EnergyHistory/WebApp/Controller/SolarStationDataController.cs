using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.SolarStation.Data;
using CS4N.EnergyHistory.WebApp.Services;
using CS4N.EnergyHistory.WebApp.ViewModels.Station;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Controller
{
  [Route("api/[controller]")]
  public sealed class SolarStationDataController : ControllerBase
  {
    public SolarStationDataController(ILogger logger, IDataStore dataStore)
    {
      service = new SolarStationDataService(logger, dataStore);
    }

    private SolarStationDataService service;

    [HttpGet("init")]
    public IActionResult GetInitData()
      => service.GetInitData();

    [HttpPost("{stationGuid}")]
    public IActionResult GetStationViewData(string stationGuid, [FromBody] ChartDataFilter filter)
      => service.GetStationViewData(stationGuid, filter);

    [HttpGet("{stationGuid}/edit")]
    public IActionResult GetSolarStationDataForEdit(string stationGuid)
      => service.GetSolarStationDataForEdit(stationGuid);

    [HttpGet("{stationGuid}/template/{year}")]
    public IActionResult GetSolarStationDataTemplate(string stationGuid, int year)
      => service.GetSolarStationDataTemplate(stationGuid, year);

    [HttpPost("{stationGuid}/edit")]
    public IActionResult PostSolarStationDataForEdit([FromBody] DataSummary data)
      => service.PostSolarStationDataForEdit(data);

    [HttpPost("{stationGuid}/import/fritzbox/csv/years")]
    public IActionResult PostImportDataFromFritzBoxAsCsvOfYears(string stationGuid, [FromForm] object file)
      => service.PostImportDataFromFritzBoxAsCsvOfYears(stationGuid, file);
  }
}