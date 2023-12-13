using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.WebApp.Repositories
{
  internal sealed class StationDataRepository : RepositoryBase
  {
    internal StationDataRepository(IDataStore dataStore) : base(dataStore) { }

    internal StationDefinition? GetStation(int id)
      => DataStore.GetStationDefinition(id);

    internal StationData GetStationData(double stationId)
      => DataStore.GetStationData(stationId);
  }
}