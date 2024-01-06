using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.WebApp.ViewModels.Cockpit;
using CS4N.EnergyHistory.WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using CS4N.EnergyHistory.Contracts.Models.Data;

namespace CS4N.EnergyHistory.WebApp.Services
{
  internal sealed class CockpitService : ServiceBase
  {
    internal CockpitService(ILogger logger, IDataStore dataStore) : base(logger)
    {
      definitionRepository = new StationDefinitionRepository(dataStore);
      dataRepository = new StationDataRepository(dataStore);
    }

    private StationDefinitionRepository definitionRepository;
    private StationDataRepository dataRepository;

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
      var stations = definitionRepository.GetStations();

      List<GenericTileData> result = [];

      foreach (var station in stations)
      {
        var data = dataRepository.GetStationData(station.Guid);

        result.Add(new GenericTileData("stations", station.Name, "StationData")
        {
          NavigationParameterAsJsonText = JsonSerializer.Serialize(new { guid = station.Guid }),
          IconUrl = station.IconUrl,
          Kpi = new KpiData
          {
            Value = Math.Round(data.GeneratedElectricityAmount).ToString(),
            ValueColor = "Good",
            Unit = station.CapacityUnit
          }
        }); ;
      }

      return result;
    }
  }
}