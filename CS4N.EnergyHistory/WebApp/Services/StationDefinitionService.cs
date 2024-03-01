using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.Contracts.Models.Station.Definition;
using CS4N.EnergyHistory.WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Services
{
  internal sealed class StationDefinitionService : ServiceBase
  {
    internal StationDefinitionService(ILogger logger, IDataStore dataStore) : base(logger)
    {
      repository = new StationDefinitionRepository(dataStore);
    }

    private StationDefinitionRepository repository;

    internal IActionResult GetStations()
    {
      logger.LogInformation("Load all stations");
      List<StationDefinition> collection = repository.GetStations();

      return new OkObjectResult(collection);
    }

    internal IActionResult GetStation(string guid)
    {
      logger.LogInformation($"Load station '{guid}'");
      StationDefinition? station = repository.GetStation(guid);

      if (station == null)
        return new OkObjectResult(new ActionReply("message_StationNotFound"));

      return new OkObjectResult(station);
    }

    internal IActionResult AddStation(StationDefinition station)
    {
      logger.LogInformation($"Add station '{station.Name}'");
      ActionReply actionReply = repository.AddStation(station);

      if (!actionReply.Successful)
        logger.LogError(actionReply.ErrorMessage);
      else
        logger.LogInformation($"Station added with guid '{station.Guid}'");

      return new OkObjectResult(actionReply);
    }

    internal IActionResult UpdateStation(StationDefinition station)
    {
      logger.LogInformation($"Update station '{station.Guid}'");
      ActionReply actionReply = repository.UpdateStation(station);

      if (!actionReply.Successful)
        logger.LogError(actionReply.ErrorMessage);

      return new OkObjectResult(actionReply);
    }

    internal IActionResult DeleteStation(StationDefinition station)
    {
      logger.LogInformation($"Delete station '{station.Guid}'");
      ActionReply actionReply = repository.DeleteStation(station);

      if (!actionReply.Successful)
        logger.LogError(actionReply.ErrorMessage);

      return new OkObjectResult(actionReply);
    }
  }
}