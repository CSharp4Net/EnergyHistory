using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;
using CS4N.EnergyHistory.WebApp.Models.Cockpit;
using CS4N.EnergyHistory.WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Services
{
  internal sealed class StationDataService : ServiceBase
  {
    internal StationDataService(ILogger logger, IDataStore dataStore) : base(logger)
    {
      definitionRepository = new StationDefinitionRepository(dataStore);
      repository = new StationDataRepository(dataStore);
    }

    private StationDefinitionRepository definitionRepository;
    private StationDataRepository repository;

    internal IActionResult GetKpiValue(string stationId)
    {
      StationData data = repository.GetStationData(stationId);

      return new OkObjectResult(new KpiData
      {
        Value = data.CollectedTotal,
        Unit = "Wh"
      });
    }

    internal IActionResult GetStationData(string stationId)
    {
      StationDefinition definition = definitionRepository.GetStation(stationId)!;

      return new OkObjectResult(new {
        definition,
        data = repository.GetStationData(stationId)
      });
    }
  }
}