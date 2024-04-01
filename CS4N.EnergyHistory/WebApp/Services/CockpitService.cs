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

      List<GenericTileData> result = [];

      result.AddRange(GetStationItems(solarSolarStationDefinitions));
      //result.AddRange(GetMeterItems(electricMeterDefinitions));

      result.Add(new GenericTileData("basic", "text_MeterReadings", "ElectricMeterData")
      {
        IconUrl = "sap-icon://business-objects-experience",
        TileFooter = "text_ElectricMeter",
        Kpi = new KpiData
        {
          Value = "0",
          ValueColor = "Error",
          Unit = "kWh"
        }
      });
      result.Add(new GenericTileData("basic", "text_ManageSolarStation", "SolarStationDefinitionOverview")
      {
        IconUrl = "sap-icon://action-settings",
        TileFooter = "text_Settings",
        Kpi = new KpiData
        {
          Value = solarSolarStationDefinitions.Count.ToString()
        }
      });
      result.Add(new GenericTileData("basic", "text_ManageElectricMeters", "ElectricMeterDefinitionOverview")
      {
        IconUrl = "sap-icon://action-settings",
        TileFooter = "text_Settings",
        Kpi = new KpiData
        {
          Value = electricMeterDefinitions.Count.ToString()
        }
      });

      return result;
    }

    private List<GenericTileData> GetStationItems(List<Contracts.Models.SolarStation.Definition> definitions)
    {
      List<GenericTileData> result = [];

      foreach (var definition in definitions)
      {
        var data = solarStationRepository.GetData(definition.Guid);

        result.Add(new GenericTileData("solarStation", definition.Name, "SolarStationData")
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

    //private List<GenericTileData> GetMeterItems(List<Contracts.Models.ElectricMeter.Definition> definitions)
    //{
    //  List<GenericTileData> result = [];

    //  foreach (var definition in definitions)
    //  {
    //    var data = electricMeterRepository.GetData(definition.Guid);

    //    result.Add(new GenericTileData("meters", definition.Name, "ElectricMeterData")
    //    {
    //      NavigationParameterAsJsonText = JsonSerializer.Serialize(new { guid = definition.Guid }),
    //      IconUrl = definition.IconUrl,
    //      Kpi = new KpiData
    //      {
    //        Value = Math.Round(data.LastRecordValue).ToString(),
    //        ValueColor = "Error",
    //        Unit = definition.CapacityUnit
    //      },
    //      TileFooter = "text_Meter"
    //    }); ;
    //  }

    //  return result;
    //}
  }
}