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
    internal List<Definition> GetDefinitions()
      => DataStore.GetElectricMeterDefinitions();

    internal Definition? GetDefinition(string guid)
      => DataStore.GetElectricMeterDefinition(guid);

    internal ActionReply AddDefinition(Definition station)
    {
      List<Definition> collection = DataStore.GetElectricMeterDefinitions();

      if (collection.Any(entry => entry.Name.Equals(station.Name, StringComparison.InvariantCultureIgnoreCase) && entry.Guid != station.Guid))
        return new ActionReply("message_NameAlreadyExists", "text_ValidationFailed");

      DataStore.UpsertElectricMeterDefinition(station);

      return new ActionReply();
    }

    internal ActionReply UpdateDefinition(Definition station)
    {
      List<Definition> collection = DataStore.GetElectricMeterDefinitions();

      if (collection.Any(entry => entry.Name.Equals(station.Name, StringComparison.InvariantCultureIgnoreCase) && entry.Guid != station.Guid))
        return new ActionReply("message_NameAlreadyExists", "text_ValidationFailed");

      DataStore.UpsertElectricMeterDefinition(station);

      return new ActionReply();
    }

    internal ActionReply DeleteDefinition(Definition station)
    {
      DataStore.DeleteElectricMeterDefinition(station.Guid);
      //DataStore.DeleteElectricMeterData(station.Guid); // TODO

      return new ActionReply();
    }
    #endregion

    #region Data
    internal DataObject GetData(string guid)
      => DataStore.GetElectricMeterData(guid);

    internal DataObject SetData(DataObject data)
    {
      DataStore.UpsertElectricMeterData(data);

      return data;
    }
    #endregion
  }
}