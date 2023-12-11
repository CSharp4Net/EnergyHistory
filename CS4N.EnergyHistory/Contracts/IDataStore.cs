using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.Contracts
{
  public interface IDataStore
  {
    List<StationDefinition> GetStationDefinitions();
    StationDefinition? GetStationDefinition(int id);
    void UpsertStationDefinition(StationDefinition definition);
    void DeleteStationDefinition(int id);

    List<StationDataMonth> GetStationDataOfYear(int stationId, int year);
    StationDataMonth? GetStationDataOfMonth(int stationId, int year, int month);
    void UpsertStationDataOfMonth(StationDataMonth month);
    void DeleteStationDataOfMonth(int stationId, int year, int month);
  }
}