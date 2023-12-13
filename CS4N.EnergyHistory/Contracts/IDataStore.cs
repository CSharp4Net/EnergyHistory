using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.Contracts
{
  public interface IDataStore
  {
    List<StationDefinition> GetStationDefinitions();
    StationDefinition? GetStationDefinition(string id);
    void UpsertStationDefinition(StationDefinition definition);
    void DeleteStationDefinition(string id);

    List<StationData> GetStationDatas();
    StationData GetStationData(string stationId);
    void UpsertStationData(StationData data);
    void DeleteStationData(string stationId);
  }
}