using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.WebApp.Repositories
{
  internal sealed class StationDataRepository : RepositoryBase
  {
    internal StationDataRepository(IDataStore dataStore) : base(dataStore) { }

    internal Station? GetStation(int id)
      => DataStore.GetStation(id);
  }
}