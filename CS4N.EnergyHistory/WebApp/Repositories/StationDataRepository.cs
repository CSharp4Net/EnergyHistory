using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.WebApp.Repositories
{
  internal sealed class StationDataRepository : RepositoryBase
  {
    internal StationDataRepository(IDataStore dataStore) : base(dataStore) { }

    internal StationDefinition? GetStation(string guid)
      => DataStore.GetStationDefinition(guid);

    internal StationData GetStationData(string stationGuid)
      => DataStore.GetStationData(stationGuid);

    internal StationData SetStationData(StationData stationData)
    { 
      DataStore.UpsertStationData(stationData);

      return stationData;
    }
  }
}