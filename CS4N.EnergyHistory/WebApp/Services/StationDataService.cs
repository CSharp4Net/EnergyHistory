using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.Contracts.Models.Definition;
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

    internal IActionResult GetStation(int id)
    {
      Station? station = repository.GetStation(id);

      if (station == null)
        return new OkObjectResult(new ActionReply("message_StationNotFound"));

      return new OkObjectResult(station);
    }
  }
}