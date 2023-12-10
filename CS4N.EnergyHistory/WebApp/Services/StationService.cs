using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Services
{
  internal sealed class StationService : ServiceBase
  {
    internal StationService(ILogger logger, IDataStore dataStore) : base(logger)
    {
      repository = new StationRepository(dataStore);
    }

    private StationRepository repository;

    internal IActionResult GetStations()
    {
      List<Station> collection = repository.GetStations();

      return new OkObjectResult(collection);
    }

    internal IActionResult GetStation(int id)
    {
      Station? station = repository.GetStation(id);

      if (station == null)
        return new OkObjectResult(new ActionReply("message_StationNotFound"));

      return new OkObjectResult(station);
    }

    internal IActionResult AddStation(Station station)
    {
      ActionReply actionReply = repository.AddStation(station);

      return new OkObjectResult(actionReply);
    }

    internal IActionResult UpdateStation(Station station)
    {
      ActionReply actionReply = repository.UpdateStation(station);

      return new OkObjectResult(actionReply);
    }

    internal IActionResult DeleteStation(Station station)
    {
      ActionReply actionReply = repository.DeleteStation(station);

      return new OkObjectResult(actionReply);
    }
  }
}