using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.WebApp.Models.Cockpit;
using CS4N.EnergyHistory.WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CS4N.EnergyHistory.WebApp.Services
{
  internal sealed class CockpitService : ServiceBase
  {
    internal CockpitService(ILogger logger, IDataStore dataStore) : base(logger)
    {
      repository = new StationDefinitionRepository(dataStore);
    }

    private StationDefinitionRepository repository;

    internal IActionResult GetItemsData()
    {
      List<GenericTileData> items = [];

      items.AddRange(GetDefaultItems());
      items.AddRange(GetStationItems());

      return new OkObjectResult(items);
    }

    private List<GenericTileData> GetDefaultItems()
    {
      return [
        new GenericTileData("settings", "Stationen verwalten", "StationDefinitionOverview")
        {
          IconUrl = "sap-icon://action-settings"
        }];
    }

    private List<GenericTileData> GetStationItems()
    {
      return repository.GetStations()
        .Select(station => new GenericTileData("stations", station.Name, "StationData")
        {
          NavigationParameterAsJsonText = JsonSerializer.Serialize(new { guid = station.Guid }),
          IconUrl = station.IconUrl
        })
        .ToList();
    }
  }
}