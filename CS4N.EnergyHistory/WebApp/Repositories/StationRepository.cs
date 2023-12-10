using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Repositories
{
  internal sealed class StationRepository : RepositoryBase
  {
    internal StationRepository(IDataStore dataStore) : base(dataStore) { }

    internal List<Station> GetStations()
      => DataStore.GetStations();

    internal Station? GetStation(int id)
      => DataStore.GetStation(id);

    internal ActionReply AddStation(Station station)
    {
      List<Station> collection = DataStore.GetStations();

      if (collection.Any(entry => entry.Name.Equals(station.Name, StringComparison.InvariantCultureIgnoreCase) && entry.Id != station.Id))
        return new ActionReply("message_NameAlreadyExists", "text_ValidationFailed");

      DataStore.UpsertStation(station);

      return new ActionReply();
    }

    internal ActionReply UpdateStation(Station station)
    {
      List<Station> collection = DataStore.GetStations();

      if (collection.Any(entry => entry.Name.Equals(station.Name, StringComparison.InvariantCultureIgnoreCase) && entry.Id != station.Id))
        return new ActionReply("message_NameAlreadyExists", "text_ValidationFailed");

      DataStore.UpsertStation(station);

      return new ActionReply();
    }

    internal ActionReply DeleteStation(Station station)
    {
      DataStore.DeleteStation(station.Id);

      return new ActionReply();
    }
  }
}