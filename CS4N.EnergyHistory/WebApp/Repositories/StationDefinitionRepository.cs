using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.WebApp.Repositories
{
  internal sealed class StationDefinitionRepository : RepositoryBase
  {
    internal StationDefinitionRepository(IDataStore dataStore) : base(dataStore) { }

    internal List<StationDefinition> GetStations()
      => DataStore.GetStationDefinitions();

    internal StationDefinition? GetStation(int id)
      => DataStore.GetStationDefinition(id);

    internal ActionReply AddStation(StationDefinition station)
    {
      List<StationDefinition> collection = DataStore.GetStationDefinitions();

      if (collection.Any(entry => entry.Name.Equals(station.Name, StringComparison.InvariantCultureIgnoreCase) && entry.Id != station.Id))
        return new ActionReply("message_NameAlreadyExists", "text_ValidationFailed");

      DataStore.UpsertStationDefinition(station);

      return new ActionReply();
    }

    internal ActionReply UpdateStation(StationDefinition station)
    {
      List<StationDefinition> collection = DataStore.GetStationDefinitions();

      if (collection.Any(entry => entry.Name.Equals(station.Name, StringComparison.InvariantCultureIgnoreCase) && entry.Id != station.Id))
        return new ActionReply("message_NameAlreadyExists", "text_ValidationFailed");

      DataStore.UpsertStationDefinition(station);

      return new ActionReply();
    }

    internal ActionReply DeleteStation(StationDefinition station)
    {
      DataStore.DeleteStationDefinition(station.Id);

      return new ActionReply();
    }
  }
}