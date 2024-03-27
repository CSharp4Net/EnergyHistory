using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.WebApp.Repositories;
using CS4N.EnergyHistory.WebApp.ViewModels.Cockpit;
using System.Text.Json;

namespace CS4N.EnergyHistory.WebApp.Services
{
  internal sealed class CockpitService : ServiceBase
  {
    internal CockpitService(ILogger logger, IDataStore dataStore) : base(logger)
    {
      solarStationRepository = new SolarStationRepository(dataStore);
      electricMeterRepository = new ElectricMeterRepository(dataStore);
    }

    private SolarStationRepository solarStationRepository;
    private ElectricMeterRepository electricMeterRepository;

    internal List<GenericTileData> GetItemsData()
    {
      var solarSolarStationDefinitions = solarStationRepository.GetDefinitions();
      var electricMeterDefinitions = electricMeterRepository.GetDefinitions();

      List<GenericTileData> result = [
        new GenericTileData("settings", "Stationen verwalten", "SolarStationDefinitionOverview")
        {
          IconUrl = "sap-icon://action-settings",
          TileFooter = "text_Settings",
          Kpi = new KpiData
          {
            Value = solarSolarStationDefinitions.Count.ToString()
          }
        },
        new GenericTileData("settings", "Stromzähler verwalten", "EletricMeterDefinitionOverview")
        {
          IconUrl = "sap-icon://action-settings",
          TileFooter = "text_Settings",
          Kpi = new KpiData
          {
            Value = electricMeterDefinitions.Count.ToString()
          }
        }
      ];

      result.AddRange(GetStationItems(solarSolarStationDefinitions));
      //result.AddRange(GetStationItems(electricMeterDefinitions));

      return result;
    }

    private List<GenericTileData> GetStationItems(List<Contracts.Models.SolarStation.Definition> definitions)
    {
      List<GenericTileData> result = [];

      foreach (var definition in definitions)
      {
        var data = solarStationRepository.GetData(definition.Guid);

        result.Add(new GenericTileData("stations", definition.Name, "SolarStationData")
        {
          NavigationParameterAsJsonText = JsonSerializer.Serialize(new { guid = definition.Guid }),
          IconUrl = definition.IconUrl,
          Kpi = new KpiData
          {
            Value = Math.Round(data.GeneratedElectricityAmount).ToString(),
            ValueColor = "Good",
            Unit = definition.CapacityUnit
          },
          TileFooter = "text_Station"
        }); ;
      }

      return result;
    }
  }
}