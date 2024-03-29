﻿using CS4N.EnergyHistory.Contracts;
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

    internal IActionResult GetDefinitions()
    {
      logger.LogDebug("Load all solar station definitions");
      List<Definition> collection = repository.GetDefinitions();

      return new OkObjectResult(collection);
    }

    internal IActionResult GetDefinition(string guid)
    {
      logger.LogDebug($"Load solar station definition '{guid}'");
      Definition? station = repository.GetDefinition(guid);

      if (station == null)
        return new OkObjectResult(new ActionReply("message_SolarStationNotFound"));

      return new OkObjectResult(station);
    }

    internal IActionResult AddDefinition(Definition definition)
    {
      logger.LogInformation($"Add solar station definition '{definition.Name}'");
      ActionReply actionReply = repository.AddDefinition(definition);

      if (!actionReply.Successful)
        logger.LogError(actionReply.ErrorMessage);
      else
        logger.LogInformation($"Solar station definition added with guid '{definition.Guid}'");

      return new OkObjectResult(actionReply);
    }

    internal IActionResult UpdateDefinition(Definition definition)
    {
      logger.LogInformation($"Update solar station definition '{definition.Guid}'");
      ActionReply actionReply = repository.UpdateDefinition(definition);

      if (!actionReply.Successful)
        logger.LogError(actionReply.ErrorMessage);

      return new OkObjectResult(actionReply);
    }

    internal IActionResult DeleteDefinition(Definition definition)
    {
      logger.LogInformation($"Delete solar station definition '{definition.Guid}'");
      ActionReply actionReply = repository.DeleteDefinition(definition);

      if (!actionReply.Successful)
        logger.LogError(actionReply.ErrorMessage);

      return new OkObjectResult(actionReply);
    }
  }
}