using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter;
using CS4N.EnergyHistory.WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Services
{
  internal sealed class ElectricMeterDefinitionService : ServiceBase
  {
    internal ElectricMeterDefinitionService(ILogger logger, IDataStore dataStore) : base(logger)
    {
      repository = new ElectricMeterRepository(dataStore);
    }

    private ElectricMeterRepository repository;

    internal IActionResult GetDefinitions()
    {
      logger.LogDebug("Load all electric meter definitions");
      List<Definition> collection = repository.GetDefinitions();

      return new OkObjectResult(collection);
    }

    internal IActionResult GetDefinition(string guid)
    {
      logger.LogDebug($"Load electric meter definition '{guid}'");
      Definition? station = repository.GetDefinition(guid);

      if (station == null)
        return new OkObjectResult(new ActionReply("message_ElectricMeterNotFound"));

      return new OkObjectResult(station);
    }

    internal IActionResult AddDefinition(Definition definition)
    {
      logger.LogInformation($"Add electric meter definition '{definition.Name}'");
      ActionReply actionReply = repository.AddDefinition(definition);

      if (!actionReply.Successful)
        logger.LogError(actionReply.ErrorMessage);
      else
        logger.LogInformation($"Electric meter definition added with guid '{definition.Guid}'");

      return new OkObjectResult(actionReply);
    }

    internal IActionResult UpdateDefinition(Definition definition)
    {
      logger.LogInformation($"Update electric meter definition '{definition.Guid}'");
      ActionReply actionReply = repository.UpdateDefinition(definition);

      if (!actionReply.Successful)
        logger.LogError(actionReply.ErrorMessage);

      return new OkObjectResult(actionReply);
    }

    internal IActionResult DeleteDefinition(Definition definition)
    {
      logger.LogInformation($"Delete electric meter definition '{definition.Guid}'");
      ActionReply actionReply = repository.DeleteDefinition(definition);

      if (!actionReply.Successful)
        logger.LogError(actionReply.ErrorMessage);

      return new OkObjectResult(actionReply);
    }
  }
}