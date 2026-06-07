using CS4N.EnergyHistory.Contracts.Models.ElectricMeter;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data;
using CS4N.EnergyHistory.Contracts.Models.SolarStation;
using CS4N.EnergyHistory.Contracts.Models.SolarStation.Data;
using CS4N.EnergyHistory.DataStore.File;

namespace CS4N.EnergyHistory.MobileApp.Services
{
  /// <summary>
  /// Lokale Implementierung: liest und schreibt JSON-Dateien direkt auf dem Gerät,
  /// indem der bestehende FileStore aus dem DataStore-Projekt verwendet wird.
  /// </summary>
  public class LocalStorageService : IStorageService
  {
    // Der FileStore aus dem bestehenden Projekt erledigt die eigentliche
    // Serialisierung / Deserialisierung als JSON-Dateien.
    private readonly FileStore _store = new();

    public Task<List<SolarStationDefinition>> GetSolarStationDefinitionsAsync()
      => Task.FromResult(_store.GetSolarStationDefinitions());

    public Task<DataSummary> GetSolarStationDataAsync(string guid)
      => Task.FromResult(_store.GetSolarStationData(guid));

    public Task UpsertSolarStationDefinitionAsync(SolarStationDefinition definition)
    {
      _store.UpsertSolarStationDefinition(definition);
      return Task.CompletedTask;
    }

    public Task UpsertSolarStationDataAsync(DataSummary data)
    {
      _store.UpsertSolarStationData(data);
      return Task.CompletedTask;
    }

    public Task DeleteSolarStationDefinitionAsync(string guid)
    {
      _store.DeleteSolarStationDefinition(guid);
      return Task.CompletedTask;
    }

    public Task<List<ElectricMeterDefinition>> GetElectricMeterDefinitionsAsync()
      => Task.FromResult(_store.GetElectricMeterDefinitions());

    public Task<ElectricMeterDataObject> GetElectricMeterDataAsync(string guid)
      => Task.FromResult(_store.GetElectricMeterData(guid));

    public Task UpsertElectricMeterDefinitionAsync(ElectricMeterDefinition definition)
    {
      _store.UpsertElectricMeterDefinition(definition);
      return Task.CompletedTask;
    }

    public Task UpsertElectricMeterDataAsync(ElectricMeterDataObject data)
    {
      _store.UpsertElectricMeterData(data);
      return Task.CompletedTask;
    }

    public Task DeleteElectricMeterDefinitionAsync(string guid)
    {
      _store.DeleteElectricMeterDefinition(guid);
      return Task.CompletedTask;
    }
  }
}
