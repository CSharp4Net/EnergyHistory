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

    [HttpGet("{guid}")]
    public IActionResult GetData(string guid)
    {
      var dataView = service.GetData(guid);

      return new OkObjectResult(dataView);
    }

    [HttpPost("{guid}")]
    public IActionResult PostNewRecord(string guid, [FromBody] DataRecord record)
      => new OkObjectResult(service.PostNewRecord(guid, record));

    [HttpDelete("{guid}/record")]
    public IActionResult DeleteRecord(string guid, [FromBody] DataRecord record)
      => new OkObjectResult(service.DeleteRecord(guid, record));

    [HttpPost("{guid}/compare")]
    public IActionResult PostCompareRecords(string guid, [FromBody] DataRecord[] records)
    {
      var comparedRecords = service.CompareRecords(guid, records);

      return new OkObjectResult(comparedRecords);
    }

  }
}