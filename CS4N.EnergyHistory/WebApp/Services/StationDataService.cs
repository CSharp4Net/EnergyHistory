using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Services
{
  internal sealed class StationDataService : ServiceBase
  {
    internal StationDataService(ILogger logger, IDataStore dataStore) : base(logger)
    {
      repository = new StationDataRepository(dataStore);
    }

    private StationDataRepository repository;

    internal IActionResult GetStationDataOfMonth(int stationId, int year, int month)
    {
      StationDataMonth data = repository.GetStationDataOfMonth(stationId, year, month);

      return new OkObjectResult(data);
    }

    internal IActionResult GetStationDataOfYear(int stationId, int year)
    {
      StationDataYear data = repository.GetStationDataOfYear(stationId, year);

      return new OkObjectResult(data);
    }

    internal IActionResult AddDataOfMonth(StationDataMonth data)
    {
      ActionReply actionReply = repository.AddDataOfMonth(data);

      return new OkObjectResult(actionReply);
    }

    internal IActionResult UpdateDataOfMonth(StationDataMonth data)
    {
      ActionReply actionReply = repository.UpdateDataOfMonth(data);

      return new OkObjectResult(actionReply);
    }

    internal IActionResult DeleteDataOfMonth(StationDataMonth data)
    {
      ActionReply actionReply = repository.DeleteDataOfMonth(data);

      return new OkObjectResult(actionReply);
    }
  }
}