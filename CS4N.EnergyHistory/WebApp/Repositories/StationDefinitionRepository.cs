using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.Contracts.Models.Station.Definition;

namespace CS4N.EnergyHistory.WebApp.Repositories
{
  internal sealed class StationDefinitionRepository : RepositoryBase
  {
    internal StationDefinitionRepository(IDataStore dataStore) : base(dataStore) { }

    internal List<StationDefinition> GetStations()
      => DataStore.GetStationDefinitions();

    internal StationDefinition? GetStation(string guid)
      => DataStore.GetStationDefinition(guid);

    internal ActionReply AddStation(StationDefinition station)
    {
      List<StationDefinition> collection = DataStore.GetStationDefinitions();

      if (collection.Any(entry => entry.Name.Equals(station.Name, StringComparison.InvariantCultureIgnoreCase) && entry.Guid != station.Guid))
        return new ActionReply("message_NameAlreadyExists", "text_ValidationFailed");

      DataStore.UpsertStationDefinition(station);

      return new ActionReply();
    }

    internal ActionReply UpdateStation(StationDefinition station)
    {
      List<StationDefinition> collection = DataStore.GetStationDefinitions();

      if (collection.Any(entry => entry.Name.Equals(station.Name, StringComparison.InvariantCultureIgnoreCase) && entry.Guid != station.Guid))
        return new ActionReply("message_NameAlreadyExists", "text_ValidationFailed");

      DataStore.UpsertStationDefinition(station);

      return new ActionReply();
    }

    internal ActionReply DeleteStation(StationDefinition station)
    {
      DataStore.DeleteStationDefinition(station.Guid);
      DataStore.DeleteStationData(station.Guid);

      return new ActionReply();
    }
  }
}