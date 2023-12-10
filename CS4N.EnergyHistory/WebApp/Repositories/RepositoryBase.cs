using CS4N.EnergyHistory.Contracts;

namespace CS4N.EnergyHistory.WebApp.Repositories
{
  internal abstract class RepositoryBase
  {
    internal RepositoryBase(IDataStore dataStore)
    {
      DataStore = dataStore;
    }

    protected IDataStore DataStore { get; init; }
  }
}