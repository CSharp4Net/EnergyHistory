using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.Contracts
{
  public interface IDataStore
  {
    List<StationDefinition> GetStationDefinitions();
    StationDefinition? GetStationDefinition(double id);
    void UpsertStationDefinition(StationDefinition definition);
    void DeleteStationDefinition(double id);

    List<StationData> GetStationDatas();
    StationData GetStationData(double stationId);
    void UpsertStationData(StationData data);
    void DeleteStationData(double stationId);
  }
}