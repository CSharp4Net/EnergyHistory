using CS4N.EnergyHistory.Contracts.Models.ElectricMeter;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data;

namespace CS4N.EnergyHistory.DataStore.File
{
  partial class FileStore
  {
    private const string electricMeterDefinitionsFileName = "ElectricMeters.json";

    private static List<ElectricMeterDefinition> cachedElectricMeterDefinitions = [];
    private static List<ElectricMeterDataObject> cachedElectricMeterDatas = [];

    #region Definition
    public List<ElectricMeterDefinition> GetElectricMeterDefinitions()
    {
      if (cachedElectricMeterDefinitions.Count > 0)
        return cachedElectricMeterDefinitions;

      return cachedElectricMeterDefinitions = LoadDefinitionsFile<ElectricMeterDefinition>(electricMeterDefinitionsFileName) ?? [];
    }

    public ElectricMeterDefinition? GetElectricMeterDefinition(string guid)
    {
      var definition = GetElectricMeterDefinitions()
        .SingleOrDefault(entry => entry.Guid.Equals(guid));

      return definition;
    }

    public void UpsertElectricMeterDefinition(ElectricMeterDefinition definition)
    {
      var definitions = GetElectricMeterDefinitions();

      var savedDefinition = definitions.SingleOrDefault(entry => entry.Guid.Equals(definition.Guid));
      if (savedDefinition == null || string.IsNullOrEmpty(definition.Guid))
      {
        definition.Guid = Guid.NewGuid().ToString();
        definition.CreatedAt = DateTime.Now;
        definitions.Add(definition);
      }
      else
      {
        int savedIndex = definitions.IndexOf(savedDefinition);

        definitions.RemoveAt(savedIndex);

        definition.UpdatedAt = DateTime.Now;

        definitions.Insert(savedIndex, definition);
      }

      WriteDefinitionsFile(definitions, electricMeterDefinitionsFileName);

      cachedElectricMeterDefinitions.Clear();
    }

    public void DeleteElectricMeterDefinition(string guid)
    {
      var definitions = GetElectricMeterDefinitions();

      definitions.RemoveAll(entry => entry.Guid == guid);

      WriteDefinitionsFile(definitions, electricMeterDefinitionsFileName);

      cachedElectricMeterDefinitions.Clear();
    }
    #endregion

    #region Data
    public List<ElectricMeterDataObject> GetElectricMeterDatas()
    {
      if (cachedElectricMeterDatas.Count > 0)
        return cachedElectricMeterDatas;

      var definitions = GetSolarStationDefinitions();

      cachedElectricMeterDatas = [];

      foreach (var definition in definitions)
        cachedElectricMeterDatas.Add(GetElectricMeterData(definition.Guid));

      return cachedElectricMeterDatas;

    }

    public ElectricMeterDataObject GetElectricMeterData(string guid)
    {
      var data = cachedElectricMeterDatas.SingleOrDefault(entry => entry.Guid == guid);
      if (data != null)
        return data;

      data = LoadDataFile<ElectricMeterDataObject>(guid);

      data ??= new ElectricMeterDataObject
      {
        Guid = guid
      };

      cachedElectricMeterDatas.Add(data);

      return data;
    }

    public void UpsertElectricMeterData(ElectricMeterDataObject data)
    {
      WriteDataFile(data);

      cachedElectricMeterDatas.Clear();
    }

    public void DeleteElectricMeterData(string guid)
    {
      DeleteDataFile<ElectricMeterDataObject>(guid);

      cachedElectricMeterDatas.Clear();
    }
    #endregion
  }
}