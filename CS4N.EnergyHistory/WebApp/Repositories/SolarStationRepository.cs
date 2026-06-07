using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.Contracts.Models.SolarStation;
using CS4N.EnergyHistory.Contracts.Models.SolarStation.Data;

namespace CS4N.EnergyHistory.WebApp.Repositories
{
  internal sealed class SolarStationRepository : RepositoryBase
  {
    internal SolarStationRepository(IDataStore dataStore) : base(dataStore) { }

    #region Definition
    internal List<SolarStationDefinition> GetDefinitions()
      => DataStore.GetSolarStationDefinitions();

    internal SolarStationDefinition? GetDefinition(string guid)
      => DataStore.GetSolarStationDefinition(guid);

    internal ActionReply AddDefinition(SolarStationDefinition station)
    {
      List<SolarStationDefinition> collection = DataStore.GetSolarStationDefinitions();

      if (collection.Any(entry => entry.Name.Equals(station.Name, StringComparison.InvariantCultureIgnoreCase) && entry.Guid != station.Guid))
        return new ActionReply("message_NameAlreadyExists", "text_ValidationFailed");

      DataStore.UpsertSolarStationDefinition(station);

      return new ActionReply();
    }

    internal ActionReply UpdateDefinition(SolarStationDefinition station)
    {
      List<SolarStationDefinition> collection = DataStore.GetSolarStationDefinitions();

      if (collection.Any(entry => entry.Name.Equals(station.Name, StringComparison.InvariantCultureIgnoreCase) && entry.Guid != station.Guid))
        return new ActionReply("message_NameAlreadyExists", "text_ValidationFailed");

      DataStore.UpsertSolarStationDefinition(station);

      return new ActionReply();
    }

    internal ActionReply DeleteDefinition(SolarStationDefinition station)
    {
      DataStore.DeleteSolarStationDefinition(station.Guid);
      DataStore.DeleteSolarStationData(station.Guid);

      return new ActionReply();
    }
    #endregion

    #region Data
    internal DataSummary GetData(string guid)
      => DataStore.GetSolarStationData(guid);

    internal DataSummary SetData(DataSummary data)
    {
      DataStore.UpsertSolarStationData(data);

      return data;
    }
    #endregion
  }
}