using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data;
using CS4N.EnergyHistory.WebApp.Services;
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
    { 
      var dataView = service.GetDatas();

      return new OkObjectResult(dataView);
    }

    [HttpGet("data/{guid}")]
    public IActionResult GetData(string guid)
    {
      var dataView = service.GetData(guid);

      return new OkObjectResult(dataView);
    }

    [HttpPost]
    public IActionResult PostData([FromBody] DataObject data)
      => new OkObjectResult(service.PostData(data));
  }
}