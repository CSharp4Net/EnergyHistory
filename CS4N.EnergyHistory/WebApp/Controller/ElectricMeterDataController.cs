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

    [HttpGet]
    public IActionResult GetData()
    { 
      var dataView = service.GetData();

      return new OkObjectResult(dataView);
    }

    [HttpPost]
    public IActionResult PostData([FromBody] DataObject data)
      => new OkObjectResult(service.PostData(data));
  }
}