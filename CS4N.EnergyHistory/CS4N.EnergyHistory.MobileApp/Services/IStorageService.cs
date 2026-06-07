
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data;
using CS4N.EnergyHistory.Contracts.Models.SolarStation;
using CS4N.EnergyHistory.Contracts.Models.SolarStation.Data;

namespace CS4N.EnergyHistory.MobileApp.Services
{
  /// <summary>
  /// Abstraktion für den Datenzugriff. Ermöglicht es, Daten entweder lokal oder
  /// über eine Netzwerkadresse/URL zu lesen und zu schreiben.
  /// </summary>
  public interface IStorageService
  {
    // --- Solar-Stationen ---
    Task<List<SolarStationDefinition>> GetSolarStationDefinitionsAsync();
    Task<DataSummary
      > GetSolarStationDataAsync(string guid);
    Task UpsertSolarStationDefinitionAsync(SolarStationDefinition definition);
    Task UpsertSolarStationDataAsync(DataSummary data);
    Task DeleteSolarStationDefinitionAsync(string guid);

    // --- Stromzähler ---
    Task<List<ElectricMeterDefinition>> GetElectricMeterDefinitionsAsync();
    Task<ElectricMeterDataObject> GetElectricMeterDataAsync(string guid);
    Task UpsertElectricMeterDefinitionAsync(ElectricMeterDefinition definition);
    Task UpsertElectricMeterDataAsync(ElectricMeterDataObject data);
    Task DeleteElectricMeterDefinitionAsync(string guid);
  }
}
