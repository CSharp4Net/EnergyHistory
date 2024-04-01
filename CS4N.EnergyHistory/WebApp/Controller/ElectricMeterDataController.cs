using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data;
using CS4N.EnergyHistory.WebApp.Services;
using CS4N.EnergyHistory.WebApp.ViewModels.ElectricMeter;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Controller
{
  [Route("api/[controller]")]
  public sealed class ElectricMeterDataController : ControllerBase
  {
    public ElectricMeterDataController(ILogger logger, IDataStore dataStore)
    {
      service = new ElectricMeterDataService(logger, dataStore);
    }

    private ElectricMeterDataService service;

    [HttpGet("init")]
    public IActionResult GetInitData()
      => service.GetInitData();

    [HttpPost("{guid}")]
    public IActionResult GetData(string guid, [FromBody] ChartDataFilter filter)
      => service.GetData(guid, filter);

    [HttpGet("{guid}/edit")]
    public IActionResult GetSolarStationDataForEdit(string guid)
      => service.GetDataForEdit(guid);

    [HttpGet("{guid}/template/{year}")]
    public IActionResult GetDataTemplate(string guid, int year)
      => service.GetDataTemplate(guid, year);

    [HttpPost("{guid}/edit")]
    public IActionResult PostDataForEdit([FromBody] DataSummary data)
      => service.PostDataForEdit(data);
  }
}