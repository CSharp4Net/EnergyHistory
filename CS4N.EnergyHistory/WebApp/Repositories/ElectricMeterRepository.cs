using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data;

namespace CS4N.EnergyHistory.WebApp.Repositories
{
  internal sealed class ElectricMeterRepository : RepositoryBase
  {
    internal ElectricMeterRepository(IDataStore dataStore) : base(dataStore) { }

    #region Definition
    internal List<ElectricMeterDefinition> GetDefinitions()
      => DataStore.GetElectricMeterDefinitions();

    internal ElectricMeterDefinition? GetDefinition(string guid)
      => DataStore.GetElectricMeterDefinition(guid);

    internal ActionReply AddDefinition(ElectricMeterDefinition station)
    {
      List<ElectricMeterDefinition> collection = DataStore.GetElectricMeterDefinitions();

      if (collection.Any(entry => entry.Name.Equals(station.Name, StringComparison.InvariantCultureIgnoreCase) && entry.Guid != station.Guid))
        return new ActionReply("message_NameAlreadyExists", "text_ValidationFailed");

      DataStore.UpsertElectricMeterDefinition(station);

      return new ActionReply();
    }

    internal ActionReply UpdateDefinition(ElectricMeterDefinition station)
    {
      List<ElectricMeterDefinition> collection = DataStore.GetElectricMeterDefinitions();

      if (collection.Any(entry => entry.Name.Equals(station.Name, StringComparison.InvariantCultureIgnoreCase) && entry.Guid != station.Guid))
        return new ActionReply("message_NameAlreadyExists", "text_ValidationFailed");

      DataStore.UpsertElectricMeterDefinition(station);

      return new ActionReply();
    }

    internal ActionReply DeleteDefinition(ElectricMeterDefinition station)
    {
      DataStore.DeleteElectricMeterDefinition(station.Guid);
      //DataStore.DeleteElectricMeterData(station.Guid); // TODO

      return new ActionReply();
    }
    #endregion

    #region Data
    internal ElectricMeterDataObject GetData(string guid)
      => DataStore.GetElectricMeterData(guid);

    internal ElectricMeterDataObject SetData(ElectricMeterDataObject data)
    {
      DataStore.UpsertElectricMeterData(data);

      return data;
    }
    #endregion
  }
}