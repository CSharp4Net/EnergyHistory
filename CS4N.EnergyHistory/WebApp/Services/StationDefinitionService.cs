using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.Contracts.Models.Definition;
using CS4N.EnergyHistory.WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

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

    internal IActionResult GetStation(string id)
    {
      logger.LogInformation($"Load station '{id}'");
      StationDefinition? station = repository.GetStation(id);

      if (station == null)
        return new OkObjectResult(new ActionReply("message_StationNotFound"));

      return new OkObjectResult(station);
    }

    internal IActionResult AddStation(StationDefinition station)
    {
      logger.LogInformation($"Add station '{station.Name}'");
      ActionReply actionReply = repository.AddStation(station);
      logger.LogInformation($"Station '{station.Id}' added");

      return new OkObjectResult(actionReply);
    }

    internal IActionResult UpdateStation(StationDefinition station)
    {
      logger.LogInformation($"Update station '{station.Id}'");
      ActionReply actionReply = repository.UpdateStation(station);

      return new OkObjectResult(actionReply);
    }

    internal IActionResult DeleteStation(StationDefinition station)
    {
      logger.LogInformation($"Update station '{station.Id}'");
      ActionReply actionReply = repository.DeleteStation(station);

      return new OkObjectResult(actionReply);
    }
  }
}