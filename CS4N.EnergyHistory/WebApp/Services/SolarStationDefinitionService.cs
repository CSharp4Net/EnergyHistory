using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.Contracts.Models.SolarStation;
using CS4N.EnergyHistory.WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Services
{
  internal sealed class SolarStationDefinitionService : ServiceBase
  {
    internal SolarStationDefinitionService(ILogger logger, IDataStore dataStore) : base(logger)
    {
      repository = new SolarStationRepository(dataStore);
    }

    private SolarStationRepository repository;

    internal IActionResult GetStations()
    {
      logger.LogInformation("Load all stations");
      List<Definition> collection = repository.GetDefinitions();

      return new OkObjectResult(collection);
    }

    internal IActionResult GetStation(string guid)
    {
      logger.LogInformation($"Load station '{guid}'");
      Definition? station = repository.GetDefinition(guid);

      if (station == null)
        return new OkObjectResult(new ActionReply("message_StationNotFound"));

      return new OkObjectResult(station);
    }

    internal IActionResult AddStation(Definition station)
    {
      logger.LogInformation($"Add station '{station.Name}'");
      ActionReply actionReply = repository.AddDefinition(station);

      if (!actionReply.Successful)
        logger.LogError(actionReply.ErrorMessage);
      else
        logger.LogInformation($"Station added with guid '{station.Guid}'");

      return new OkObjectResult(actionReply);
    }

    internal IActionResult UpdateStation(Definition station)
    {
      logger.LogInformation($"Update station '{station.Guid}'");
      ActionReply actionReply = repository.UpdateDefinition(station);

      if (!actionReply.Successful)
        logger.LogError(actionReply.ErrorMessage);

      return new OkObjectResult(actionReply);
    }

    internal IActionResult DeleteStation(Definition station)
    {
      logger.LogInformation($"Delete station '{station.Guid}'");
      ActionReply actionReply = repository.DeleteDefinition(station);

      if (!actionReply.Successful)
        logger.LogError(actionReply.ErrorMessage);

      return new OkObjectResult(actionReply);
    }
  }
}