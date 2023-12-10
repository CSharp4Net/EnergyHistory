using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.WebApp.Models.Cockpit;
using CS4N.EnergyHistory.WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CS4N.EnergyHistory.WebApp.Services
{
  internal sealed class CockpitService : ServiceBase
  {
    internal CockpitService(ILogger logger, IDataStore dataStore) : base(logger)
    {
      repository = new StationDefinitionRepository(dataStore);
    }

    private StationDefinitionRepository repository;

    internal IActionResult GetItems()
    {
      List<GenericTileData> items = [];

      var stationSettings = new GenericTileData("settings", "Stationen", "StationDefinitionOverview");
      var stationTiles = repository.GetStations()
        .Select(station => new GenericTileData("stations", station.Name, "Station")
        {
          NavigationParameterAsJsonText = JsonSerializer.Serialize(new { id = station.Id })
        })
        .ToList();

      stationSettings.KpiValue = stationTiles.Count.ToString();

      items.Add(stationSettings);
      items.AddRange(stationTiles);

      return new OkObjectResult(items);
    }
  }
}